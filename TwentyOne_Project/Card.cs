using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyOne_Project
{
    public struct Card
    {
        public Suit Suit { get; set; }

        public Face Face { get; set; }

        // Custom ToString() that will print the Face and Suit of a card to the console when called
        public override string ToString()
        {
            return string.Format("{0} of {1}", Face, Suit + " ");
        }
    }

    public enum Suit
    {
        Clubs,
        Diamonds,
        Hearts,
        Spades
    }

    public enum Face
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}
