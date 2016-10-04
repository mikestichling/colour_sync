using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using ColourSync.Domain.Enums;

namespace ColourSync.Domain
{
    public class GameState
    {
        public GameState()
        {
            this.Tables = new List<Table>();
        }

        public void JoinTable(string table, string player, Guid id)
        {
            if (!Tables.Any(t => t.Name == table))
            {
                this.AddTable(table);
                Tables.Single(t => t.Name == table).AddPlayer(new Player(player, id));
            }
            else if (Tables.Single(t => t.Name == table).Players.Any(p => p.Id == id))
            {
                Tables.Single(t => t.Name == table).Players.Single(p => p.Id == id).Name = player;
            }
            else
            {
                Tables.Single(t => t.Name == table).AddPlayer(new Player(player, id));
            }
            
        }

        public void LeaveTable(string table, Guid id)
        {
            if (!Tables.Any(t => t.Name == table))
            {
                return;
            }

            Tables.Single(t => t.Name == table).RemovePlayer(id);
        }

        public void AddTable(string name)
        {
            this.AddTable(new Table(name));
        }

        public void AddTable(Table table)
        {
            this.Tables.Add(table);
        }

        public void RemoveTable(string name)
        {
            var thisTable = Tables.Single(t => t.Name == name);
            Tables.Remove(thisTable);
        }

        public void MakeMoveForPlayer(string onTable, string playerName, Moves move, DateTime timestamp)
        {
            var table = Tables.Single(t => t.Name == onTable);
            var player = table.Players.Single(p => p.Name == playerName);
            player.MakeMove(move, timestamp);
        }

        public List<Table> Tables {get; private set;}
    }
}