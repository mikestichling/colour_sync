using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using ColourSync.Domain.Enums;

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

        public void RemovePlayer(Guid id)
        {
            var thisPlayer = this.Players.Single(p => p.Id == id);

            this.Players.Remove(thisPlayer);
        }

        public GameConfiguration StartGame(int seed)
        {
            CurrentRound = 1;

            var moves = GetRandomMoves(seed);

            return new GameConfiguration(moves);
        }

        public GameConfiguration NextRound(int seed)
        {
            if (Players.Any(p => p.Move == null))
                throw new InvalidOperationException("Cannot advance to the next round if not all players have made their move");

            Players.ForEach(p => p.ResetMove());
            CurrentRound++;

            var moves = GetRandomMoves(seed);

            return new GameConfiguration(moves);
        }

        private List<Moves> GetRandomMoves(int seed)
        {
            var random = new Random(seed);
            var numberOfPlayer = Players.Count() - 1 > 4 ? Players.Count() - 1 : 4;
            var moves = new List<Moves>();
            var count = 0;
            while(count < numberOfPlayer)
            {
                var index = random.Next(0, 20);
                var move = (Moves)index;
                if (!moves.Contains(move) && move != Moves.Joker)
                {
                    moves.Add(move);
                    count++;
                }
            }

            return moves;
        }

        public string Name {get; private set;}
        public List<Player> Players {get; private set;}

        public int CurrentRound {get; private set;}

        public int PlayersNeedingToMove 
        {
            get
            {
                return Players.Count(p => p.Move == null);
            }
        }
        
        private List<Move> MovesForCurrentRound
        {
            get
            {
                return Players.Select(p => p.Move).ToList();
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

                var filtered = ordered.Where(g => g.Count() == mostSelected && g.Count() != 1).ToList();
                var list = new List<Player>();
                
                foreach(var kvp in filtered)
                {
                    list.AddRange(this.Players.Where(p => p.Move.ChosenMove == kvp.Key));
                }

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
                    return Players.Where(p => p.Id != player.Id).ToList();
                }
                else
                {
                    return MovesForCurrentRound.Where(m => m.ChosenMove == Enums.Moves.Joker).Select(m => m.Player).ToList();
                }
                
            }
        }

        public List<MoveBreakdown> MoveBreakdowns 
        {
            get
            {
                var colourGroups = MovesForCurrentRound.GroupBy(m => m.ChosenMove);
                var ordered = colourGroups.OrderByDescending(g => g.Count());

                var result = new List<MoveBreakdown>();

                var list = new List<Player>();
                
                foreach(var kvp in ordered)
                {
                    result.Add(new MoveBreakdown(kvp.Key, this.Players.Where(p => p.Move.ChosenMove == kvp.Key).ToList()));
                }

                return result;
            }
        }
    }
}