namespace JackalWebHost2.Controllers.Models.Lobby;

public class AssignTeamRequest
{
    public string LobbyId { get; set; }
    
    public long UserId { get; set; }
    
    public long? TeamId { get; set; }
}