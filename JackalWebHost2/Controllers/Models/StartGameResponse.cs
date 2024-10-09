using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models;

public class StartGameResponse
{
    public string GameName { get; set; }
    
    public List<PirateChange> Pirates { get; set; }
    
    public DrawMap Map { get; set; }
    
    public int MapId { get; set; }
    
    public GameStatistics Stats { get; set; }
    
    public List<DrawMove> Moves { get; set; }
}