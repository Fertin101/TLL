using System.ComponentModel.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TLL
{
    internal class Program
    {
        static void NTurnament()

        {
            string teamA, teamB;
            int delkaZapasu;
            string vyherce = "";
            char  switchOdpoved;
            string vyherceZapas = "";

            Console.Clear();

            string jmeno = Utility.TextValidace("Jmeno Turnaje");
            string liga = Utility.TextValidace("Liga");
            string zeme = Utility.TextValidace("Zeme konani");
            string mesto = Utility.TextValidace("Mesto konani");
            DateTime datum = Utility.DateValidace("Zadejte datum");

            Console.Clear();

            int pocetZapasu = Utility.CisloValidace("Pocet zapasu",20);

            string[] teamAZaznam = new string[pocetZapasu];
            string[] teamBZaznam = new string[pocetZapasu];
            string[] vyherceZapasZaznam = new string[pocetZapasu];
            int[] minutyZaznam = new int[pocetZapasu];

            for (int i = 0; i < pocetZapasu; i++)
            {
                Console.Clear();

                bool TeamValid = false;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Prave zapisujete hodnoty pro zapas {i + 1}:");
                Console.ForegroundColor = ConsoleColor.White;

                teamA = Utility.TextValidace("Team A");
                teamAZaznam[i] = teamA;
                teamB = Utility.TextValidace("Team B");
                teamBZaznam[i] = teamB;

                Console.WriteLine("-----------Zvolte VYHERCE Zapasu------------");
                Console.WriteLine("[A]" +teamA + "\n[B]" + teamB + "\n[D] REMIZA/ZAPAS ZRUSEN");
                do
                {
                    switchOdpoved = char.ToUpper(Console.ReadKey().KeyChar);
                    switch (switchOdpoved)
                    {
                        case 'A':
                            vyherceZapas = teamA;
                            vyherceZapasZaznam[i] =teamA ;
                            TeamValid=true;
                            break;
                        case 'B':
                            vyherceZapas = teamB;
                            vyherceZapasZaznam[i] = teamB;
                            TeamValid = true;
                            break;
                        case 'D':
                            vyherceZapasZaznam[i] ="DNF";
                            TeamValid = true;
                            break;
                        default:
                            Utility.ErrorZprava("Zvolte jednu z moznosti!");
                            break;
                    }
                } while (!TeamValid);

                delkaZapasu = Utility.CisloValidace("Delka zapasu v minutach", 120);
                minutyZaznam[i] = delkaZapasu;
            }

            bool CheckIfTrue=false;
            do
            {
                Console.Clear();

                for (int i = 0; i < pocetZapasu; i++)
                {
                    Console.WriteLine($"{i + 1}. {vyherceZapasZaznam[i]}");
                }

                 int volbaVyherce = Utility.CisloValidace($"Cislo Vyherce [1-{pocetZapasu}]", pocetZapasu);
                if (volbaVyherce <= pocetZapasu && volbaVyherce>0) { 
                     vyherce = vyherceZapasZaznam[volbaVyherce - 1];
                CheckIfTrue = true;
                }
                else
                {
                    CheckIfTrue=false;
                }
            } while (!CheckIfTrue);

            XmlData.XML( jmeno,  liga,  mesto,  zeme,  datum,  teamAZaznam,  teamBZaznam,  vyherceZapasZaznam,  minutyZaznam);
    }   
    static void Main(string[] args)
        
        {

            char odpoved;
            do {
            Console.Clear();
            Console.WriteLine("ISLoL v.1 ");
            Console.WriteLine("NOVY TURNAMENT - [N]");
            Console.WriteLine("VYPSAT TURNAMENT -  [V]");
            Console.WriteLine("VYMAZAT TURNAMENT - [R]");
                 
              odpoved =  char.ToUpper(Console.ReadKey().KeyChar);
                switch(odpoved)
                {  

                    case 'N':
                      
                        NTurnament();
                        break;

                    case 'V':
                       
                        break;

                    case 'W':

                        break;

                default:

                        Utility.ErrorZprava("Zvolte spravnou moznost");
                        break;
                } 

            } while (odpoved != 'K');
        }
    }
}
