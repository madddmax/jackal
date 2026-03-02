namespace JackalWebHost2.Models;

public class TurnGameModel
{
    public long GameId { get; set; }
    
    public int? MoveNum { get; set; }
    
    public Guid? PirateId { get; set; }
    
    public int? TurnNumber { get; set; }
}