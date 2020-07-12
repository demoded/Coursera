using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Declare a variable for a random number generator and create a new Random object for that variable
            Random rand = new Random();

            //Declare other variables as necessary to keep track of each player’s roll, 
            int playerDie;
            int compDie;

            //each player’s number of wins, and whether or not we should play another game of War
            int playerWin;
            int compWin;
            Char wantPlay = 'y';

            //Print a “welcome” message to the user telling them that the program will play games of War
            Console.WriteLine("Welcome to WAR!!!\n");

            //While the player wants to play another game
            while (wantPlay == 'y' || wantPlay == 'Y')
            {
                //reset game status
                playerWin = 0;
                compWin = 0;

                for (int i = 0; i < 21; i++)
                {
                    //generate new random numbers
                    playerDie = rand.Next(13) + 1;
                    compDie = rand.Next(13) + 1;

                    //while values are equal print WAR and generate new values
                    while (playerDie == compDie)
                    {
                        Console.Write("   WAR! P1:{0:D}\t P2:{1:D}\t\n", playerDie, compDie);
                        playerDie = rand.Next(13) + 1;
                        compDie = rand.Next(13) + 1;
                    }

                    //write battle data
                    Console.Write("BATTLE: P1:{0:D}\t P2:{1:D}\t", playerDie, compDie);

                    //determine winner and print his
                    if (playerDie > compDie)
                    {
                        Console.Write("P1 Wins!\n");
                        playerWin += 1;
                    }
                    else
                    {
                        Console.Write("P2 Wins!\n");
                        compWin += 1;
                    }
                }

                //print final winner and result
                if (playerWin > compWin)
                    Console.WriteLine("\nP1 is the overall Winner with {0:D} battles!\n", playerWin);
                else
                    Console.WriteLine("\nP2 is the overall Winner with {0:D} battles!\n", compWin);

                

                    //Ask to play again. Accept only N key any other key means YES
                    Console.Write("Do you want to play again (y/n)?");
                if (Console.ReadKey().Key == ConsoleKey.N)
                    wantPlay = 'n';
                else
                    wantPlay = 'y';
                
                //empty line for better output
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
