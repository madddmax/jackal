using Jackal.Core;
using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models;

public class LoadGameResponse
{
    public long GameId { get; set; }
    
    public GameModeType GameMode { get; set; }
    
    public string TilesPackName { get; set; }
    
    public List<PirateChange> Pirates { get; set; }
    
    public DrawMap Map { get; set; }
    
    public int MapId { get; set; }
    
    public GameStatistics Stats { get; set; }
    
    public List<DrawTeam> Teams { get; set; }
    
    public List<DrawMove> Moves { get; set; }
}