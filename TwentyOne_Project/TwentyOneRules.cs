using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne_Project
{
    public class TwentyOneRules
    {
        // Private and static Dictionary to track values of cards (static to avoid having to create a TwentyOneRules object when using)
        private static Dictionary<Face, int> _cardValues = new Dictionary<Face, int>()
        {
            [Face.Two] = 2,
            [Face.Three] = 3,
            [Face.Four] = 4,
            [Face.Five] = 5,
            [Face.Six] = 6,
            [Face.Seven] = 7,
            [Face.Eight] = 8,
            [Face.Nine] = 9,
            [Face.Ten] = 10,
            [Face.Jack] = 10,
            [Face.Queen] = 10,
            [Face.King] = 10,
            [Face.Ace] = 1
        };

        private static int[] GetAllPossibleHandValues(List<Card> Hand)
        {
            // Created in video 5 of putting it all together series
            // Uses a lambda expression to find out how many Aces are in the player's hand
            int aceCount = Hand.Count(x => x.Face == Face.Ace);
            int[] result = new int[aceCount + 1];
            // Takes the value of the cards in a hand from the Dictionary and sums it
            int value = Hand.Sum(x => _cardValues[x.Face]);
            result[0] = value;

            if (result.Length == 1)
            {
                return result;
            }

            for (int i = 1; i < result.Length; i++)
            {
                value = value + (i * 10);
                result[i] = value;
            }

            return result;
        }

        // Checks to see if the player or dealer has won immediately
        public static bool CheckForBlackJack(List<Card> Hand)
        {
            int[] possibleValues = GetAllPossibleHandValues(Hand);
            int value = possibleValues.Max();
            if (value == 21)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        // Checks to see if the player has busted (aka. gotten a hand value of over 21)
        public static bool IsBusted(List<Card> Hand)
        {
            int value = GetAllPossibleHandValues(Hand).Min();

            if (value > 21)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        // Determines whether or not the Dealer should Stay or Hit
        public static bool ShouldDealerStay(List<Card> Hand)
        {
            int[] possibleHandValues = GetAllPossibleHandValues(Hand);

            foreach (int value in possibleHandValues)
            {
                if (value > 16 && value < 22)
                {
                    return true;
                }
            }

            return false;
        }

        // Takes in a nullable boolean
        public static bool? CompareHands(List<Card> PlayerHand, List<Card> DealerHand)
        {
            int[] playerResults = GetAllPossibleHandValues(PlayerHand);
            int[] dealerResults = GetAllPossibleHandValues(DealerHand);

            // Asking for a list of player/dealer results where the iteger is less than 22 and then get the largest of those results
            int playerScore = playerResults.Where(x => x < 22).Max();
            int dealerScore = dealerResults.Where(x => x < 22).Max();

            if (playerScore > dealerScore)
            {
                return true;
            }

            else if (playerScore < dealerScore)
            {
                return false;
            }

            else
            {
                return null;
            }
        }
    }
}
