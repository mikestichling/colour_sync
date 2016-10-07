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
            this.Move = null;
        }
        public string Name {get; set;}

        public Guid Id {get; private set;}

        public Move Move {get; private set;}

        public void ResetMove()
        {
            this.Move = null;
        }
        internal void MakeMove(Moves chosenMove, DateTime timestamp)
        {
            this.Move = new Move(chosenMove, timestamp, new Player(this.Name, this.Id));
        }
    }
}