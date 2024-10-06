namespace JackalWebHost2.Models.Lobby;

public class LobbyMember
{
    public long UserId { get; set; }
    
    public string UserName { get; set; }
    
    public long? TeamId { get; set; }
    
    public DateTimeOffset LastSeen { get; set; }
    
    public DateTimeOffset JoinedAt { get; set; }
}