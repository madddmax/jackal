using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Models.Lobby;

public class CreateLobbyRequest
{
    public GameSettings Settings { get; set; }
}