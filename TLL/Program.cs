using static System.Net.Mime.MediaTypeNames;
using System.Globalization;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace LoLTurnaje
{

    internal class Program
    {
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
        static TTurnament Pridat()
        {
            TTurnament turnament = new();

            Console.Clear();

            turnament.jmeno = TextValidace("Jmeno turnaje");
            turnament.liga = TextValidace("Jmeno ligy");
            turnament.zeme = TextValidace("Zeme konani");
            turnament.mesto = TextValidace("Mesto konani");
            turnament.datum = DatumValidace("Datum zacatku konani");

            Console.Clear();

            turnament.pocetZap = CisloValidace("Pocet zapasu v turnaji", 15);

            turnament.teamA = new string[turnament.pocetZap];
            turnament.teamB = new string[turnament.pocetZap];
            turnament.delkaZap = new int[turnament.pocetZap];
            turnament.vyherceZap = new string[turnament.pocetZap];

            for (int i = 0; i < turnament.pocetZap; i++)
            {
                Console.Clear();

                bool teamCheck = false;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Prave zapisujete hodnoty pro zapas {i + 1}:");
                Console.WriteLine("Pocet zapasu " + turnament.pocetZap);
                Console.ForegroundColor = ConsoleColor.White;

                string A = TextValidace("Nazev tymu A");
                turnament.teamA[i] = A;
                string B = TextValidace("Nazev tymu B");
                turnament.teamB[i] = B;

                Console.WriteLine("-----------Zvolte VYHERCE Zapasu------------");
                Console.WriteLine("[A]" + A + "\n[B]" + B + "\n[D] REMIZA/ZAPAS ZRUSEN");
                do
                {
                    char switchOdpoved = char.ToUpper(Console.ReadKey().KeyChar);
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
                            turnament.vyherceZap[i] = "DNF";
                            teamCheck = true;
                            break;
                        default:
                            ErrorZprava("Zvolte jednu z moznosti!");
                            break;
                    }
                } while (!teamCheck);

                int delka = CisloValidace("Delka zapasu v minuach", 120);
                turnament.delkaZap[i] = delka;
            }

            bool vyherceCheck = false;
            do
            {
                Console.Clear();

                for (int i = 0; i < turnament.pocetZap; i++)
                {
                    Console.WriteLine($"{i + 1}. {turnament.vyherceZap[i]}");
                }

                int volbaVyh = CisloValidace($"Cislo Vyherce Turnamentu [1-{turnament.pocetZap}]", turnament.pocetZap);
                if (volbaVyh <= turnament.pocetZap && volbaVyh > 0)
                {
                    turnament.vyherceTur = turnament.vyherceZap[volbaVyh - 1];
                    vyherceCheck = true;
                }

            } while (!vyherceCheck);

            return turnament;
        }

        static void CtiXml()
        {


        }

        static string TextValidace(string text = "")
        {
            string input = "";
            bool check = true;
            while (check)
            {
                Console.WriteLine("\nZadejte " + text + ":");
                input = Console.ReadLine() ?? "";
                if (string.IsNullOrWhiteSpace(input))
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

        static DateTime DatumValidace(string text = "")
        {
            DateTime datum;
            bool isValid;
            do

            {
                Console.Write(text + "(DD/MM/YYYY):");
                isValid = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum);
                if (!isValid)
                {
                    ErrorZprava("Prosim zadejte spravne datum napr.(20/08/2024)");
                }
            } while (!isValid);
            return datum;

        }

        static int CisloValidace(string text = "", int max = int.MaxValue)
        {
            int input;
            bool check;
            do
            {
                Console.WriteLine("\nZadejte " + text + ":");
                check = int.TryParse(Console.ReadLine(), out input);
                if (!check || input > max || input < 1)
                {
                    ErrorZprava("Zadejte prosim cislo, ktere je mensi nebo rovno " + max);
                    check = false;
                }

            } while (!check);
            return input;
        }
        static void ErrorZprava(string text = "")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Main(string[] args)
        {
            char odpoved;
            do
            {
                Console.Clear();
                Console.WriteLine("EVIDENCE LoL Turnaju ver.1");
                Console.WriteLine("--------------------------");
                Console.WriteLine("[N] - NOVY TURNAMENT");
                Console.WriteLine("[V] - VYPSAT TURNAMENTY");
                Console.WriteLine("[O] - UMISTENI SOUBORU");
                Console.WriteLine("[K] - UKONCIT PROGRAM");
                Console.WriteLine("--------------------------");

                odpoved = char.ToUpper(Console.ReadKey().KeyChar);
                switch (odpoved)
                {

                    case 'N':
                        Pridat();

                        Console.WriteLine("Turnaj byl úspěšně přidán.");
                        Console.ReadKey();

                        break;
                    case 'K':
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        ErrorZprava("Zvolte spravnou moznost");
                        Console.ReadKey();
                        break;
                }

            } while (odpoved != 'K');


        }
    }
}
