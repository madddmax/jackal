using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models;

public class LobbyModel
{
    public string Id { get; set; } = "";
    
    public long OwnerId { get; set; }

    public Dictionary<long, LobbyMemberModel> LobbyMembers { get; set; } = new();
    
    public GameSettings GameSettings { get; set; }
    
    public int NumberOfPlayers { get; set; }
    
    public string? GameId { get; set; }
}