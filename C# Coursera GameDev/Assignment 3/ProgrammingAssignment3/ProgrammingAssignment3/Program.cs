using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleCards;

namespace ProgrammingAssignment3
{
    class Program
    {

        static void Main(string[] args)
        {
            //Declare variables for and create a deck of cards and blackjack hands for the dealer and the player
            Deck deck = new Deck();
            BlackjackHand playerHand = new BlackjackHand("Player");
            BlackjackHand dealerHand = new BlackjackHand("Dealer");

            //Print a “welcome” message to the user telling them that the program will play a single hand of Blackjack
            Console.WriteLine("Hi Player, let's play one hand of BlackJack.");
            
            //Shuffle the deck of cards
            deck.Shuffle();

            //Deal 2 cards to the player and dealer
            playerHand.AddCard(deck.TakeTopCard());
            playerHand.AddCard(deck.TakeTopCard());
            dealerHand.AddCard(deck.TakeTopCard());
            dealerHand.AddCard(deck.TakeTopCard());

            //Make all the player’s cards face up (you need to see what you have!); there's a method for this in the BlackjackHand class
            playerHand.ShowAllCards();

            //Make the dealer’s first card face up (the second card is the dealer’s “hole” card); there's a method for this in the BlackjackHand class            
            dealerHand.ShowFirstCard();

            //Print both the player’s hand and the dealer’s hand
            Console.Write("Player: ");
            playerHand.Print();
            Console.Write("Dealer: ");
            dealerHand.Print();

            //Let the player hit if they want to
            playerHand.HitOrNot(deck);

            //Make all the dealer’s cards face up; there's a method for this in the BlackjackHand class
            dealerHand.ShowAllCards();

            //Print both the player’s hand and the dealer’s hand
            Console.Write("Player: ");
            playerHand.Print();
            Console.Write("Dealer: ");
            dealerHand.Print();

            //Print the scores for both hands
            Console.WriteLine("Player score: " + playerHand.Score);
            Console.WriteLine("Dealer score: " + dealerHand.Score);
            Console.WriteLine();

            //Print win message
            if (playerHand.Score <= 21 
             && playerHand.Score > dealerHand.Score)
                Console.WriteLine("Yahooooo!!! Player won!");
            else if (dealerHand.Score <= 21 
                  && playerHand.Score < dealerHand.Score)
                Console.WriteLine("Dealer won the game :(");
            else
                Console.WriteLine("Draw.");

            //Print blank line
            Console.WriteLine();
        }
    }
}
