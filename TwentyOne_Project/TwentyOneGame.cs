using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwentyOne_Project
{
    public class TwentyOneGame : Game, IWalkAway
    {
        public TwentyOneDealer Dealer { get; set; }

        public override void Play()
        {
            Dealer = new TwentyOneDealer();
            // Checks for all of the players in the list of Players
            foreach (Player player in Players)
            {
                // Creates a new hand for the player so old hands don't carry over and confirms the player is still in the game
                player.Hand = new List<Card>();
                player.Stay = false;
            }

            // Creates a new hand for the dealer so old hands don't carry over and confirms the dealer is still in the game
            // Creates a new deck
            Dealer.Hand = new List<Card>();
            Dealer.Stay = false;
            Dealer.Deck = new Deck();
            Dealer.Deck.Shuffle();

            Console.WriteLine("Place your bet!");

            foreach (Player player in Players)
            {
                int bet = Convert.ToInt32(Console.ReadLine());
                bool successfullyBet = player.Bet(bet);

                // If statement checks Bet()
                if (!successfullyBet)
                {
                    // If successfullyBet == false, the Play() method is ended
                    return;
                }

                // If the bet is successful...
                Bets[player] = bet;
            }

            // Gives every player two cards
            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Dealing...");

                foreach (Player player in Players)
                {
                    Console.Write("{0}: ", player.Name);
                    Dealer.Deal(player.Hand);
                    
                    // Checks to see if the player has Blackjack - player gets an Ace and a Jack card
                    if (i == 1)
                    {
                        // Pass in the player's hand with CheckForBlackJack() to check for BlackJack
                        // TwentyOneRules is a static method so must be prefaced by class name
                        bool blackJack = TwentyOneRules.CheckForBlackJack(player.Hand);

                        // Executes if CheckForBlackJack() is true (player wins immediately)
                        if (blackJack)
                        {
                            Console.WriteLine("BlackJack! {0} wins {1}", player.Name, Bets[player]);
                            // Player wins bet multiplied by 1.5 and their initial bet
                            player.Balance += Convert.ToInt32((Bets[player] * 1.5) + Bets[player]);
                            return;
                        }
                    }
                }

                // Checks to see if the Dealer has BlackJack
                Console.Write("Dealer: ");
                Dealer.Deal(Dealer.Hand);
                if (i == 1)
                {
                    bool blackJack = TwentyOneRules.CheckForBlackJack(Dealer.Hand);

                    if (blackJack)
                    {
                        Console.WriteLine("Dealer has BlackJack! Everyone loses!");
                        foreach (KeyValuePair<Player, int> entry in Bets)
                        {
                            Dealer.Balance += entry.Value;
                        }

                        return;
                    }
                }
            }

            // Loop to ask each player if they want to Hit or Stay
            foreach (Player player in Players)
            {
                while (!player.Stay)
                {
                    Console.WriteLine("Your cards are: ");

                    foreach (Card card in player.Hand)
                    {
                        Console.Write("{0}", card.ToString());
                    }

                    Console.WriteLine("\n\nHit or Stay?");
                    string answer = Console.ReadLine().ToLower();

                    if (answer == "stay")
                    {
                        player.Stay = true;
                        break;
                    }

                    else if (answer == "hit")
                    {
                        Dealer.Deal(player.Hand);
                    }

                    bool busted = TwentyOneRules.IsBusted(player.Hand);

                    if (busted)
                    {
                        Dealer.Balance += Bets[player];
                        Console.WriteLine("{0} Busted! You lose your bet of {1}. Your balance is now {2}.", player.Name, Bets[player], player.Balance);
                        Console.WriteLine("Do you want to play again?");
                        answer = Console.ReadLine().ToLower();

                        if (answer == "yes" || answer == "yeah" || answer == "yea")
                        {
                            player.isActivelyPlaying = true;
                            return;
                        }

                        else
                        {
                            player.isActivelyPlaying = false;
                            return;
                        }
                    }
                }
            }

            Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);
            Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);

            while (!Dealer.Stay && !Dealer.isBusted)
            {
                Console.WriteLine("Dealer is hitting...");
                Dealer.Deal(Dealer.Hand);
                Dealer.isBusted = TwentyOneRules.IsBusted(Dealer.Hand);
                Dealer.Stay = TwentyOneRules.ShouldDealerStay(Dealer.Hand);
            }

            if (Dealer.Stay)
            {
                Console.WriteLine("Dealer is staying.");
            }

            if (Dealer.isBusted)
            {
                Console.WriteLine("Dealer Busted!");

                // Foreach loop to determine how much each Player wins if the Dealer loses
                foreach (KeyValuePair<Player, int> entry in Bets)
                {
                    Console.WriteLine("{0} won {1}!", entry.Key.Name, entry.Value);

                    // Lambda expression that gives each player their balance of winnings
                    Players.Where(x => x.Name == entry.Key.Name).First().Balance += (entry.Value * 2);

                    // Minus bet from the Dealer
                    Dealer.Balance -= entry.Value;
                }

                return;
            }

            // Dealer stayed and Player stayed but no one busted -- need logic to compare Player logic vs the Dealer to see who has the higher hand
            foreach (Player player in Players)
            {
                // Boolean with three possibilities! -- to make a bool null add ? to the end which makes the boolean nullable
                bool? playerWon = TwentyOneRules.CompareHands(player.Hand, Dealer.Hand);

                // If no one wins -- it's a tie
                if (playerWon == null)
                {
                    Console.WriteLine("Push! No one wins.");
                    player.Balance += Bets[player];
                }

                else if (playerWon == true)
                {
                    Console.WriteLine("{0} won {1}!", player.Name, Bets[player]);
                    player.Balance += (Bets[player] * 2);
                    Dealer.Balance -= Bets[player];
                }

                else
                {
                    Console.WriteLine("Dealer wins {0}!", Bets[player]);
                    Dealer.Balance += Bets[player];
                }


                Console.WriteLine("Play again?");
                string answer = Console.ReadLine().ToLower();

                if (answer == "yes" || answer == "yeah")
                {
                    player.isActivelyPlaying = true;
                }

                else
                {
                    player.isActivelyPlaying = false;
                }
            }
        }

        public override void ListPlayers()
        {
            Console.WriteLine("21 Players: ");
            base.ListPlayers();
        }

        public void WalkAway(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
