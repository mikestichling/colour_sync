using System.Collections.Generic;
using ColourSync.Domain;
using ColourSync.Domain.Enums;

public class MoveBreakdown
{
    public MoveBreakdown(Moves move, List<Player> players)
    {
        this.Move = move.ToString().ToLowerInvariant();;
        this.Players = players;
    }
    public string Move {get; private set;}

    public List<Player> Players {get; private set;}
}