using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroToStrings
{
    class Program
    {
        /// <summary>
        /// Demonstrates string basics
        /// </summary>
        /// <param name="args">command-line arguments</param>
        static void Main(string[] args)
        {   
            //promt and read in gamer tag
            Console.Write("Enter gamertag please: ");
            string gamertag = Console.ReadLine();

            //promt and read in level
            Console.Write("Enter level: ");
            int level = int.Parse(Console.ReadLine());

            //extract first letter of gamertag
            char firstGamertagChar = gamertag[0];

            //read a csv string
            Console.Write("Enter name and percent (name,percent): ");
            string csvString = Console.ReadLine();

            //find comma location
            int commaLocation = csvString.IndexOf(",");

            //extract name and percent
            string name = csvString.Substring(0, commaLocation);
            float percent = float.Parse(csvString.Substring(commaLocation + 1));

            //print out values
            Console.WriteLine();
            Console.WriteLine("Gameryag: " + gamertag);
            Console.WriteLine("Level: " + level);
            Console.WriteLine("First Gamertag character: " + firstGamertagChar);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("Percent: " + percent);
        }
    }
}
