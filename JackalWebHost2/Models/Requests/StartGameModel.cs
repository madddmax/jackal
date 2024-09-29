namespace JackalWebHost2.Models.Requests;

public class StartGameModel
{
    public string GameName { get; set; }
    public GameSettings Settings { get; set; }
}