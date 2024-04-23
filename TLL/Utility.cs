using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TLL
{
    internal class Utility
    {
        public static void ErrorZprava(string text)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;  
        }
       public static string TextValidace(string text)
        {
            string input = "null";
            bool check = true;
            while (check)
            {
                Console.WriteLine("\nZadejte " + text + ":");
                input = Console.ReadLine();
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

       public static int CisloValidace(string text, int max=int.MaxValue)
        {
            int input;
            bool check;
            do
            {
                Console.WriteLine("\nZadejte " + text + ":");
                string userInput = Console.ReadLine();
                check = int.TryParse(userInput, out input);
                if (!check || input > max || input < 1)
                {
                    ErrorZprava("Zadejte prosim cislo, ktere je mensi nebo rovno " + max);
                    check = false;
                }

            } while (!check);
            return input;
        }

        public static DateTime DateValidace(string text)

        {
            DateTime datum;
            string inputCislo;
            bool isValid;
            do

            {
                Console.Write(text + "(DD/MM/YYYY):");
                inputCislo = Console.ReadLine();
                isValid = DateTime.TryParseExact(inputCislo, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum);
                if (!isValid)
                {
                    Utility.ErrorZprava("Prosim zadejte spravne datum (20/08/2024)");
                }
            } while (!isValid);
            return datum;

        }



    }
}
