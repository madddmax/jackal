namespace JackalWebHost2.Controllers.Requests;

public class TurnGameRequest
{
    public string GameName { get; set; }
    
    public int? TurnNum { get; set; }
    
    public Guid? PirateId { get; set; }
}