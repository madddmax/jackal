
using Jackal.Core;

namespace JackalWebHost2.Models;

public class StartGameResult
{
    public string GameName { get; init; }
    
    public GameModeType GameMode { get; init; }
    
    public List<PirateChange> Pirates { get; init; }
    
    public DrawMap Map { get; init; }
    
    public int MapId { get; init; }
    
    public GameStatistics Statistics { get; init; }
    
    public List<DrawMove> Moves { get; init; }
}