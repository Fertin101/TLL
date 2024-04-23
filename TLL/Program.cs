using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace TLL
{
    internal class Program
    {      
        static void NTurnament()

        {
            string jmeno, liga, mesto, zeme,vyherce,teamA, teamB;
            int pocetZapasu, delkaZapasu;
            DateTime datum;
            bool  TeamValid=false;
            char  switchOdpoved;
            string vyherceZapas = "";

            Console.Clear();

            jmeno = Utility.TextValidace("Jmeno Turnaje");
            liga = Utility.TextValidace("Liga");
            zeme = Utility.TextValidace("Zeme konani");
            mesto = Utility.TextValidace("Mesto konani");
            datum = Utility.DateValidace("Zadejte datum");
            pocetZapasu = Utility.CisloValidace("Pocet zapasu");

            string[] teamAZaznam = new string[pocetZapasu];
            string[] teamBZaznam = new string[pocetZapasu];
            string[] vyherceZapasZaznam = new string[pocetZapasu];


            for (int i = 0; i < pocetZapasu; i++)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Prave zapisujete hodnoty pro zapas {i + 1}:");
                Console.ForegroundColor = ConsoleColor.White;

                teamA = Utility.TextValidace("Team A");
                teamAZaznam[i] = teamA;
                teamB = Utility.TextValidace("Team B");
                teamBZaznam[i] = teamB;

                Console.WriteLine("-----------Zvolte VYHERCE Zapasu------------");
                Console.WriteLine("[A] - " +teamA + " [B] - "+teamB + " [D] -REMIZA/HRA ZRUSENA");
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

              

                delkaZapasu = Utility.CisloValidace("Delka zapasu v minutach");      
            }


            Console.WriteLine("\nInformace o turnaji:");
            Console.WriteLine("Jmeno Turnaje: " + jmeno);
            Console.WriteLine("Liga: " + liga);
            Console.WriteLine("Zeme konani: " + zeme);
            Console.WriteLine("Mesto konani: " + mesto);
            Console.WriteLine("Datum konani: " + datum.ToString("dd/MM/yyyy"));
            Console.WriteLine("Pocet zapasu: " + pocetZapasu);
            Console.WriteLine("Vyherce: " + vyherce);
            Console.WriteLine("\nVýsledky zápasů:");
            for (int i = 0; i < pocetZapasu; i++)
            {
                Console.WriteLine($"Zápas {i + 1}: Team A - {teamAZaznam[i]}, Team B - {teamBZaznam[i]}, Vítěz - {vyherceZapasZaznam[i]}");
            }


            Console.WriteLine("\nStiskněte libovolnou klávesu pro pokračování...");
            Console.ReadKey(true); // Čeká na stisknutí klávesy, než se pokračuje
        

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
