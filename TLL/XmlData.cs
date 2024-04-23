using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace TLL
{
    internal class XmlData
    {
        static public void XML(string vyherce ,string jmeno, string liga, string mesto, string zeme, int pocetZapasu, DateTime datum, string[] teamAZaznam, string[] teamBZaznam, string[] vyherceZapasZaznam, int[] minutyZaznam)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true; 

            string directoryPath = @"C:\Users\frant\Documents";
            string filePath = Path.Combine(directoryPath, $"XML_K_{jmeno}.xml");

            using (XmlWriter writer = XmlWriter.Create(filePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Turnament");
                writer.WriteElementString("Nazev", jmeno);
                writer.WriteElementString("Vyherce", vyherce);
                writer.WriteStartElement("Lokace");
                writer.WriteElementString("Zeme", zeme);
                writer.WriteElementString("Mesto", mesto);
                writer.WriteEndElement(); // Konec lokace
                writer.WriteElementString("Datum", datum.ToString("yyyy-MM-dd"));
                writer.WriteStartElement("Zapasy");
                writer.WriteElementString("Pocet_zapasu", pocetZapasu.ToString());

                for (int i = 0; i < pocetZapasu; i++)
                {
                    writer.WriteStartElement("Zapas"); 
                    writer.WriteAttributeString("Cislo", (i+1).ToString());
                    writer.WriteElementString("Tym_A", teamAZaznam[i]);
                    writer.WriteElementString("Tym_B", teamBZaznam[i]);
                    writer.WriteElementString("Vitez", vyherceZapasZaznam[i]);
                    writer.WriteElementString("Delka", minutyZaznam[i].ToString());
                    writer.WriteEndElement(); // konec zapasu
                }

                writer.WriteEndElement(); // konec zapasy
                writer.WriteEndElement(); // konec turnament
                writer.WriteEndDocument();

                writer.Flush();
            }
    }
}
}
