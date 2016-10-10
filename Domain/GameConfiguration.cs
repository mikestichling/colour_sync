using System.Collections.Generic;
using System.Linq;
using ColourSync.Domain.Enums;

public class GameConfiguration
{
    public GameConfiguration(List<Moves> moves)
    {
        this.Colours = moves.Select(m => m.ToString().ToLowerInvariant()).ToList();
    }

    public List<string> Colours { get; private set; }
}