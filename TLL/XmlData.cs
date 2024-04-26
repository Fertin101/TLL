namespace TLL
{
    using System;
    using System.IO;
    using System.Xml.Linq; // Tento using direktiv zahrnuje potřebné třídy pro práci s XML

    internal class XmlData
    {
        // Metoda pro přidání nového turnaje do XML souboru
        public static void XML(string vyherce, string jmeno, string liga, string mesto, string zeme, int pocetZapasu, DateTime datum, string[] teamAZaznam, string[] teamBZaznam, string[] vyherceZapasZaznam, int[] minutyZaznam)
        {
            string directoryPath = @"C:\Users\frant\Documents";
            string filePath = Path.Combine(directoryPath, "TurnamentData.xml");

            // Inicializace XML dokumentu a kořenového elementu
            XDocument doc;
            XElement root;

            // Kontrola, zda soubor existuje
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath); // Načtení existujícího dokumentu
                root = doc.Element("Turnaje") ?? new XElement("Turnaje"); // Použij existující nebo vytvoř nový kořen
            }
            else
            {
                doc = new XDocument(); // Vytvoř nový dokument, pokud neexistuje
                root = new XElement("Turnaje"); // Vytvoř kořenový element "Turnaje"
                doc.Add(root); // Přidej kořenový element do dokumentu
            }

            // Vytvoření elementu turnaje s všemi detaily
            XElement turnaj = new XElement("Turnaj",
                new XElement("Jmeno", jmeno),
                new XElement("Liga", liga),
                new XElement("Vyherce", vyherce),
                new XElement("Lokace",
                    new XElement("Zeme", zeme),
                    new XElement("Mesto", mesto)
                ),
                new XElement("Datum", datum.ToString("yyyy-MM-dd")),
                new XElement("Zapasy", new XAttribute("Pocet", pocetZapasu.ToString()))
            );

            // Přidání detailů o zápasech do turnaje
            for (int i = 0; i < pocetZapasu; i++)
            {
                XElement zapas = new XElement("Zapas",
                    new XAttribute("Cislo", i + 1),
                    new XElement("Tym_A", teamAZaznam[i]),
                    new XElement("Tym_B", teamBZaznam[i]),
                    new XElement("Vyherce", vyherceZapasZaznam[i]),
                    new XElement("Delka_trvani_minuty", minutyZaznam[i].ToString())
                );
                turnaj.Element("Zapasy").Add(zapas); // Přidej každý zápas do "Zapasy" v turnaji
            }

            root.Add(turnaj); // Přidej kompletní turnaj do kořenového elementu
            doc.Save(filePath); // Ulož dokument na specifikovanou cestu
        }
    }
}
