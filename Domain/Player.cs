using System;
using System.Collections.Generic;
using ColourSync.Domain.Enums;

namespace ColourSync.Domain
{
    public class Player
    {
        public Player(string name, Guid id)
        {
            this.Name = name;
            this.Id = id;
            this.Moves = new List<Move>();
        }
        public string Name {get; set;}

        public Guid Id {get; private set;}

        public List<Move> Moves {get; private set;}

        internal void MakeMove(Moves chosenMove, DateTime timestamp)
        {
            var move = new Move(chosenMove, timestamp, new Player(this.Name, this.Id));
            Moves.Add(move);
        }
    }
}