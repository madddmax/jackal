namespace JackalWebHost2.Controllers.Models;

public class TurnGameRequest
{
    public long GameId { get; set; }
    
    public int? TurnNum { get; set; }
    
    public Guid? PirateId { get; set; }
}