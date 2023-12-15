using System;
using System.Collections.Generic;
using System.Text;

namespace TwentyOne_Project
{
   public abstract class Game
    {
        // Adding private to avoid the System.NullReferenceException error
        // Need to create an empty list to avoid null error but technically new list and get; set; method are the same thing
        private List<Player> _players = new List<Player>();
        private Dictionary<Player, int> _bets = new Dictionary<Player, int>();

        public List<Player> Players { get { return _players; } set { _players = value; } }
        public string Name { get; set; }
        public Dictionary<Player, int> Bets { get { return _bets; } set { _bets = value; }  }

        public abstract void Play(); // This is a method, not a property. Abstract methods MUST be used with the abstract class it is in.

        public virtual void ListPlayers()
        {
            foreach (Player player in Players)
            {
                Console.WriteLine(player.Name);
            }
        }
    }
}
