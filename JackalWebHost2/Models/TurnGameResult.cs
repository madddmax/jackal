namespace JackalWebHost2.Models;

public class TurnGameResult
{
    public List<PirateChange> PirateChanges { get; init; }
    
    public List<TileChange> Changes { get; init; }
    
    public GameStatistics Statistics { get; init; }
    
    public List<DrawTeam> Teams { get; set; }
    
    public List<DrawMove> Moves { get; init; }
}