namespace JackalWebHost2.Models.Lobby;

public class Lobby
{
    public string Id { get; set; } = "";
    
    public long OwnerId { get; set; }

    public Dictionary<long, LobbyMember> LobbyMembers { get; set; } = new();
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public DateTimeOffset? ClosedAt { get; set; }
    
    public GameSettings GameSettings { get; set; }
    
    public int NumberOfPlayers { get; set; }
    
    public string? GameId { get; set; }
}