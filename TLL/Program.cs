using System.IO;
using System;
using System.Linq;
using System.Globalization;
using System.Xml.Linq;
using System.Xml.Schema;


namespace TLL
{

    internal class Program
    {
        // Struktura pro uchovávání informací o turnaji
        struct TTurnament
        {
            public string jmeno;
            public string liga;
            public string zeme;
            public string mesto;

            public string[] teamA;
            public string[] teamB;
            public string vyherceTur;
            public string[] vyherceZap;
            public int pocetZap;
            public int[] delkaZap;
            public DateTime datum;

        }
        // Cesty k souborům XML a XSD
        private static string cestaXml = Path.Combine(Directory.GetCurrentDirectory(), "Data.xml");
        private static string cestaXsd = Path.Combine(Directory.GetCurrentDirectory(), "Schema.xsd");
       
        // Metoda pro přidání nového turnaje
        static TTurnament Pridat()
        {
            TTurnament turnament = new();
            try
            {
                Console.Clear();
                // Získání a validace základních informací o turnaji
                turnament.jmeno = TextValidace("Jmeno turnaje");
                turnament.liga = TextValidace("Jmeno ligy");
                turnament.zeme = TextValidace("Zeme konani");
                turnament.mesto = TextValidace("Mesto konani");
                turnament.datum = DatumValidace("Datum zacatku konani");

                Console.Clear();
                // Získání a validace počtu zápasů
                turnament.pocetZap = CisloValidace("Pocet zapasu v turnaji", 15);

                // Inicializace polí pro týmy, délky zápasů a vítěze zápasů velikost pole je podle počtu zápasů 
                turnament.teamA = new string[turnament.pocetZap];
                turnament.teamB = new string[turnament.pocetZap];
                turnament.delkaZap = new int[turnament.pocetZap];
                turnament.vyherceZap = new string[turnament.pocetZap];

                // Získání a validace informací o jednotlivých zápasech
                for (int i = 0; i < turnament.pocetZap; i++) // opakuj pro každý zápas
                {
                    Console.Clear();

                    bool teamCheck = false;

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"Prave zapisujete hodnoty pro zapas {i + 1}:");
                    Console.WriteLine("Pocet zapasu " + turnament.pocetZap);
                    Console.ForegroundColor = ConsoleColor.White;
                   
                    // Získání a validace názvů týmů A a B
                    string A = TextValidace("Nazev tymu A");
                    turnament.teamA[i] = A;
                    string B = TextValidace("Nazev tymu B");
                    turnament.teamB[i] = B;
                    //menu pro výběr vítěze zápasu, lze zvolit pouze předem dáné tymy
                    Console.WriteLine("-----------Zvolte VYHERCE Zapasu------------");
                    Console.WriteLine("[A]" + A + "\n[B]" + B + "\n[D] ZAPAS ZRUSEN");
                    do
                    {
                        char switchOdpoved = char.ToUpper(Console.ReadKey().KeyChar); // Čtení vstupu od uživatele a konverze na velké písmeno
                        switch (switchOdpoved) 
                        {
                            case 'A':
                                turnament.vyherceZap[i] = A;
                                teamCheck = true;
                                break;
                            case 'B':
                                turnament.vyherceZap[i] = B;
                                teamCheck = true;
                                break;
                            case 'D':
                                turnament.vyherceZap[i] = "ZRUSENO";
                                teamCheck = true;
                                break;
                            default:
                                ErrorZprava("Zvolte jednu z moznosti!");
                                break;
                        }
                    } while (!teamCheck);
                    //  Získání a validace délky zápasu
                    int delka = CisloValidace("Delka zapasu v minuach", 120);
                    turnament.delkaZap[i] = delka;
                }
                // Výběr vítěze turnaje z vítězů zápasů
                bool vyherceCheck = false;
                do
                {
                    Console.Clear();

                    for (int i = 0; i < turnament.pocetZap; i++)  // Smyčka pro zobrazení všech vítězů zápasů
                    {
                        Console.WriteLine($"{i + 1}. {turnament.vyherceZap[i]}"); // Zobrazení vítězů jednotlivých zápasů
                    }

                    int volbaVyh = CisloValidace($"Cislo Vyherce Turnamentu [1-{turnament.pocetZap}]", turnament.pocetZap);
                    if (volbaVyh <= turnament.pocetZap && volbaVyh > 0)
                    {
                        turnament.vyherceTur = turnament.vyherceZap[volbaVyh - 1]; // Nastavení vítěze turnaje
                        vyherceCheck = true;
                    }

                } while (!vyherceCheck);
                return turnament;
            }
            catch (Exception e)
            {
                ErrorZprava($"Chyba pri vytvareni turnaje: {e.Message}");
                return default;
            }
        }
        // Metoda pro smazání turnaje z XML souboru
        static void SmazXml()
        {
            try
            {
                if (File.Exists(cestaXml))
                {
                    // Načtení XML dokumentu
                    XDocument doc = XDocument.Load(cestaXml);
                    ValidaceXml(doc); // Validace XML dokumentu
                    var turnaje = doc.Descendants("Turnaj").ToList(); // Načtení seznamu turnajů z XML

                    Console.Clear();

                    for (int i = 0; i < turnaje.Count; i++)
                    {
                        Console.WriteLine($"{i + 1} - {turnaje[i].Element("Jmeno")?.Value} "); // Zobrazení pořadí a názvu turnaje
                    }
                    int volbaSmaz = CisloValidace("Napiste cislo turnaje ktere chcete smazat", turnaje.Count);

                    var jmenoTurnaje = turnaje[volbaSmaz - 1].Element("Jmeno")?.Value;
                    ErrorZprava($"\n[Y] - Doopravdy chcete smazat {jmenoTurnaje}");
                    char odpovedY = char.ToUpper(Console.ReadKey().KeyChar); // Kontrola, zda uživatel chce doopravdy smazat turnaj
                    if (odpovedY == 'Y')
                    {
                        turnaje[volbaSmaz - 1].Remove(); // Smazání vybraného turnaje z XML dokumentu
                        doc.Save(cestaXml);
                        Console.WriteLine($"\nTurnaj {jmenoTurnaje} byl smazan ");
                    }
                    else
                    {
                        ErrorZprava("\nMazani zruseno");
                    }
                }
                else
                {
                    Console.WriteLine("Soubor XML neexistuje");
                }
            }
            catch (Exception e)
            {
                ErrorZprava($"Chyba pri praci s XML: {e.Message}");
            }
            Console.ReadKey();
        }
        // Metoda pro načtení turnaje z XML souboru
        static void NactiXml()
        {
            try
            {
                if (File.Exists(cestaXml)) {
                    // Načtení XML dokumentu
                    XDocument doc = XDocument.Load(cestaXml);
                    ValidaceXml(doc); // Validace XML dokumentu
                    var turnaje = doc.Descendants("Turnaj").ToList(); // Načtení seznamu turnajů z XML

                    Console.Clear();



                    for (int i = 0; i < turnaje.Count; i++)  // Smyčka pro zobrazení všech turnajů
                    {
                        Console.WriteLine($"{i + 1} - {turnaje[i].Element("Jmeno")?.Value} ");
                    }
                    int volbaVypis = CisloValidace("Napiste cislo turnaje ktere chcete nacist", turnaje.Count); // Získání a validace výběru turnaje k načtení

                    // Ziskání určítého turnaje a jeho výpis
                    XElement zvolenyTurnaj = turnaje[volbaVypis - 1];
                    Console.WriteLine($"Jmeno turnaje: {zvolenyTurnaj.Element("Jmeno")?.Value}");
                    Console.WriteLine($"Liga: {zvolenyTurnaj.Element("Liga")?.Value}");
                    Console.WriteLine($"Vitez turnaje: {zvolenyTurnaj.Element("Vyherce")?.Value}");
                    Console.WriteLine($"Lokace: {zvolenyTurnaj.Element("Lokace")?.Element("Zeme")?.Value}, {zvolenyTurnaj.Element("Lokace")?.Element("Mesto")?.Value}");
                    Console.WriteLine($"Datum konani: {zvolenyTurnaj.Element("Datum")?.Value}");
                    var zapasy = zvolenyTurnaj.Descendants("Zapas").ToList();
                    foreach (var zapas in zapasy) // smyčka pro každý zapas
                    {
                        Console.WriteLine($"Zapas {zapas.Attribute("Cislo")?.Value}: " +
                            $"{zapas.Element("Tym_A")?.Value} vs {zapas.Element("Tym_B")?.Value} " +
                            $"Vitez: {zapas.Element("Vyherce")?.Value} - Delka zapasu: {zapas.Element("Delka_trvani_minuty")?.Value} minut");
                    }
                }
                else
                {
                    Console.WriteLine("Soubor XML neexistuje");
                }
                }
            catch (Exception e)
            {
                ErrorZprava($"Chyba pri praci s XML: {e.Message}");
            }
            Console.ReadKey();
        }
        // Metoda pro uložení turnaje do XML souboru
        static void UlozXml(TTurnament turnament)
        {
            try
            {
                XDocument doc;
                //pokud´ existuje XML soubor tak se načte, zda ne vytoří se nový XML soubor s kořenovým elemetem Turnaje
                if (File.Exists(cestaXml))
                {
                    doc = XDocument.Load(cestaXml);
                }
                else
                {
                    doc = new XDocument(new XElement("Turnaje"));
                }
                
            
                XElement root = doc.Element("Turnaje") ?? new XElement("Turnaje"); // Získání kořenového elementu nebo vytvoření nového
                // Vytvoření elementu "Turnaj" s detaily o turnaji
                XElement turnaj = new XElement("Turnaj",
                new XElement("Jmeno", turnament.jmeno),
                new XElement("Liga", turnament.liga),
                new XElement("Vyherce", turnament.vyherceTur),
                new XElement("Lokace",
                    new XElement("Zeme", turnament.zeme),
                    new XElement("Mesto", turnament.mesto)
                ),
                new XElement("Datum", turnament.datum.ToString("yyyy-MM-dd")),
                new XElement("Zapasy", new XAttribute("Pocet", turnament.pocetZap.ToString()))
            );
                // Přidání detailů o zápasech do turnaje
                for (int i = 0; i < turnament.pocetZap; i++)
                {
                    XElement zapas = new XElement("Zapas",
                        new XAttribute("Cislo", i + 1),
                        new XElement("Tym_A", turnament.teamA[i]),
                        new XElement("Tym_B", turnament.teamB[i]),
                        new XElement("Vyherce", turnament.vyherceZap[i]),
                        new XElement("Delka_trvani_minuty", turnament.delkaZap[i].ToString())
                    );
                    turnaj.Element("Zapasy")?.Add(zapas); ; // Přidej zápas do "Zapasy" v turnaji
                }
                root.Add(turnaj); // Přidej kompletní turnaj do kořenového elementu
                doc.Save(cestaXml); // Ulož dokument na specifikovanou cestu
                Console.WriteLine("Turnaj byl úspěšně přidán.");

            }
            catch (Exception e)
            {
                ErrorZprava($"Chyba pri ukladani turnaje: {e.Message}");
            }
        }
        // Metoda pro validaci XML dokumentu pomocí XSD schématu
        static void ValidaceXml(XDocument doc)
        {
            XmlSchemaSet schemaSet = new XmlSchemaSet(); // Vytvoření nové sady schémat XML
            schemaSet.Add("", cestaXsd); // Přidání XSD schématu do sady schémat
            bool chybaValidace = false;
            doc.Validate(schemaSet, (o, e) => //lambda validationEventHandler
            {
                ErrorZprava($"Chyba validace: {e.Message}");
                chybaValidace = true;
            });
            if (chybaValidace)
            {
                throw new Exception("dokument neodpovida schematu, nebo nemate zadny turnaj ulozen"); // Vyvolání výjimky v případě, že dokument neodpovídá schématu
            }
        }
        // Metoda pro validaci textu od uživatele
        static string TextValidace(string text)
        {
            string input = "";
            bool check = true;
            while (check)
            {
                Console.WriteLine("\nZadejte " + text + ":");
                input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input)) // pokud je input nic nebo jen mezery program se dal nepusti
                {
                    ErrorZprava("Prosim vyplnte udaj");
                }
                else
                {
                    check = false;
                }
            }
            return input;
        }
        // Metoda pro validaci data od uživatele
        static DateTime DatumValidace(string text)
        {
            DateTime datum;
            bool isValid;
            do

            {
                Console.Write(text + "(DD/MM/YYYY):");
                isValid = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum); // Parsování datumu, specifický formát dd/MM/yyyy, ingoruje nastavení pc uživatele, arsování proběhne striktně podle zadaného formátu bez jakýchkoli úprav
                if (!isValid)
                {
                    ErrorZprava("Prosim zadejte spravne datum napr.(20/08/2024)");
                }
            } while (!isValid);
            return datum;
        }
        // Metoda pro validaci čísla od uživatele
        static int CisloValidace(string text, int max)
        {
            int input;
            bool check;
            do
            {
                Console.WriteLine("\nZadejte " + text + ":");
                check = int.TryParse(Console.ReadLine(), out input); // parsování čísla
                if (!check || input > max || input < 1) // číslo musí být větší než nula a menší než zadané maximum. Musí splnit parsování
                {
                    ErrorZprava("Zadejte prosim cislo, ktere je mensi nebo rovno " + max);
                    check = false;
                }
            } while (!check);
            return input;
        }
        // Metoda pro zobrazení chybové zprávy
        static void ErrorZprava(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Nastavení barvy textu na červenou
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White; // Nastavení barvy textu zpět na bílou
        
        }
        // Hlavní metoda programu - MENU
        static void Main(string[] args)
        {
            char odpoved; // Proměnná pro uložení volby uživatele
            do
            {
                Console.Clear();
                // Zobrazení menu
                Console.WriteLine("EVIDENCE LoL Turnaju ver.1");
                Console.WriteLine("--------------------------");
                Console.WriteLine("[N] - NOVY TURNAMENT");
                Console.WriteLine("[V] - VYPSAT TURNAMENT");
                Console.WriteLine("[S] - SMAZAT TURNAMENT");
                Console.WriteLine("[K] - UKONCIT PROGRAM");
                Console.WriteLine("--------------------------");

                odpoved = char.ToUpper(Console.ReadKey().KeyChar); // Čtení vstupu od uživatele a konverze na velké písmeno

                TTurnament novyTurnaj;
                switch (odpoved)
                {

                    case 'N':
                        novyTurnaj = Pridat(); // Vytvoření nového turnaje
                        UlozXml(novyTurnaj);  // Uložení nového turnaje do XML souboru
                        Console.ReadKey();   
                        break;
                    case 'V':
                        NactiXml();
                        break;
                    case 'K':
                        Environment.Exit(0);
                        break;
                    case 'S':

                        SmazXml();
                        break;

                    default:
                        Console.Clear();
                        ErrorZprava("Zvolte spravnou moznost");
                        Console.ReadKey();
                        break;
                }

            } while (odpoved != 'K'); // Opakování cyklu dokud uživatel nezvolí ukončení programu
        }
    }
}