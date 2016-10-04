using System;
using System.Collections.Generic;
using ColourSync.Domain.Enums;

namespace ColourSync.Domain
{
    public class Player
    {
        public Player(string name)
        {
            this.Name = name;
            this.Moves = new List<Move>();
        }
        public string Name {get; private set;}

        public List<Move> Moves {get; private set;}

        internal void MakeMove(Moves chosenMove, DateTime timestamp)
        {
            var move = new Move(chosenMove, timestamp, this);
            Moves.Add(move);
        }
    }
}