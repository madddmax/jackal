using JackalWebHost2.Models;

namespace JackalWebHost2.Controllers.Requests;

public class StartGameRequest
{
    public string GameName { get; set; }
    
    public GameSettings Settings { get; set; }
}