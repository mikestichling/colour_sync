using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColourSync.Domain
{
    public class Table
    {
        private readonly char[] letters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private readonly int tableNameLength = 4;
        private readonly int seed;
        public Table(int seed) : this()
        {
            this.seed = seed;
            var sb = new StringBuilder();
            var random = new Random(this.seed);
            for(int i = 0; i < tableNameLength; i++)
            {
                int index = random.Next(0, letters.Length);
                sb.Append(letters[index]);
            }
            this.Name = sb.ToString();
        }

        public Table(string name) : this()
        {
            this.Name = name.ToLowerInvariant();
        }

        private Table()
        {
            Players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            this.Players.Add(player);
        }

        public void RemovePlayer(string name)
        {
            var thisPlayer = this.Players.Single(p => p.Name == name);

            this.Players.Remove(thisPlayer);
        }

        public void StartGame()
        {
            CurrentRound = 1;
        }

        public void NextRound()
        {
            if (Players.Any(p => p.Moves.Count < CurrentRound))
                throw new InvalidOperationException("Cannot advance to the next round if not all players have made their move");

            CurrentRound++;
        }

        public string Name {get; private set;}
        public List<Player> Players {get; private set;}

        public int CurrentRound {get; private set;}

        public int PlayersNeedingToMove 
        {
            get
            {
                return Players.Count(p => p.Moves.Count < CurrentRound);
            }
        }
        
        private List<Move> MovesForCurrentRound
        {
            get
            {
                return Players.Select(p => p.Moves[CurrentRound - 1]).ToList();
            }
        }

        public Player SlowestPlayer
        {
            get
            {
                return MovesForCurrentRound.OrderBy(m => m.Timestamp).Last().Player; 
            }
        }

        public List<Player> Loosers
        {
            get
            {
                var colourGroups = MovesForCurrentRound.GroupBy(m => m.ChosenMove);
                var ordered = colourGroups.OrderByDescending(g => g.Count());
                var mostSelected = ordered.First().Count();

                var filtered = ordered.Where(g => g.Count() == mostSelected).ToList();
                var list = new List<Player>();
                filtered.ForEach(f => f.OrderBy(o => o.Timestamp).ToList().ForEach(m => list.Add(m.Player))); 

                return list;
            }
        }

        public List<Player> JokerLoosers
        {
            get
            {
                if (MovesForCurrentRound.Where(m => m.ChosenMove == Enums.Moves.Joker).Count() == 1)
                {
                    var player = MovesForCurrentRound.Single(m => m.ChosenMove == Enums.Moves.Joker).Player;
                    return Players.Where(p => p != player).ToList();
                }
                else
                {
                    return MovesForCurrentRound.Where(m => m.ChosenMove == Enums.Moves.Joker).Select(m => m.Player).ToList();
                }
                
            }
        }
    }
}