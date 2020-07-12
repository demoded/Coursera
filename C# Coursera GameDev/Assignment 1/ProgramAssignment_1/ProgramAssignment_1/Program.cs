using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProgramAssignment_1
{
    /// <summary>
    /// A class realised program logic for Program Assignment 1
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main method of the first Programm Assignment 
        /// </summary>
        /// <param name="args">Command-line agrs</param>
        static void Main(string[] args)
        {
            // print greeting message and prompt for enter amaout of gold
            Console.WriteLine("Hi there! I'm a SUPERComputer, I can calculate your gold-collecting performance!");
            Console.Write("How much gold you've collected? ");

            // read amount of gold from console
            int gold = int.Parse(Console.ReadLine());

            // prompt how many hours played
            Console.Write("How many hours you've played? ");

            // read hours from console
            float hours = float.Parse(Console.ReadLine());

            // calculate minutes and gold per minute 
            float minutes = hours * 60;
            float goldPerMinute = gold / minutes;

            // print empyty line and title for statistics
            Console.WriteLine("");
            Console.WriteLine("Ok. Here is the result:");

            // print gold value
            Console.Write("Collected gold : ");
            Console.WriteLine(gold);

            // print hours spent
            Console.Write("Hours played   : ");
            Console.WriteLine(hours);

            // print calculated gold per minute ratio
            Console.Write("Gold per minute: ");
            Console.WriteLine(goldPerMinute);

            // print empty line and prompt no nress any key
            Console.WriteLine("");
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
