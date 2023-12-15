using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TwentyOne_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Creates a log
            // The File.ReadAllText argument could also be - C:\\Users\\megcl\\OneDrive\\Documents\\Logs\\log.txt"
            string text = File.ReadAllText(@"C:\Users\megcl\OneDrive\Documents\Logs\log.txt");

            Console.WriteLine("Welcome to the Grand Hotel and Casino. Let's start by telling me your name.");
            string playerName = Console.ReadLine();
            Console.WriteLine("And how much money did you bring today?");
            int bank = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Hello, {0}. Would you like to join a game of 21 right now?", playerName);
            string answer = Console.ReadLine().ToLower();

            // If statement checks answer variable for different types of inputs (that's why it was made .ToLower
            if (answer == "yes" || answer == "yeah" || answer == "y" || answer == "ya")
            {
                // Creates new player with inputs given above (created by the constructor)
                Player player = new Player(playerName, bank);
                // Polymorphism here (so it will expose overloaded operators)
                Game game = new TwentyOneGame();
                // Adds player to game
                game += player;
                player.isActivelyPlaying = true;

                while (player.isActivelyPlaying && player.Balance > 0)
                {
                    game.Play();
                }

                // Game ends if while loop defined above is exited
                game -= player;
                Console.WriteLine("Thank you for playing!");
            }

            // If the player answer is not a version of yes
            Console.WriteLine("Please feel free to look around the casino. Bye for now.");
            Console.Read();
        }
    }
}
