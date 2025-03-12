using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models;

public class TurnGameResponse
{
    public List<PirateChange> PirateChanges { get; set; }
    
    public List<TileChange> Changes { get; set; }
    
    public GameStatistics Stats { get; set; }
    
    public List<TeamScore> TeamScores { get; set; }
    
    public List<DrawMove> Moves { get; set; }
}