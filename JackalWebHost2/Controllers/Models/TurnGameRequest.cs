namespace JackalWebHost2.Controllers.Models;

public class TurnGameRequest
{
    public long GameId { get; set; }
    
    public int? MoveNum { get; set; }
    
    public Guid? PirateId { get; set; }
    
    public int? TurnNumber { get; set; }
}