using System;
using ColourSync.Domain.Enums;

namespace ColourSync.Domain
{
    public class Move
    {
        public Move(Moves move, DateTime timestamp, Player player)
        {
            this.ChosenMove = move;
            this.Timestamp = timestamp;
            this.Player = player;
        }

        public Moves ChosenMove {get; private set;}
        public DateTime Timestamp {get; private set;}

        public Player Player {get; private set;}
    }
}