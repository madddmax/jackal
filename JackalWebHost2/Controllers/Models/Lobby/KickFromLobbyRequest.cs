namespace JackalWebHost2.Controllers.Models.Lobby;

public class KickFromLobbyRequest
{
    public string LobbyId { get; set; }
    
    public long TargetUserId { get; set; }
}