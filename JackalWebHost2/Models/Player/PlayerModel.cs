using JackalWebHost2.Models.Map;

namespace JackalWebHost2.Models.Player;

public class PlayerModel
{
    public long Id { get; set; }
    
    public PlayerType Type { get; set; }
    
    public MapPositionId Position { get; set; }
}