namespace JackalWebHost2.Models;

public class TurnGameModel
{
    public long GameId { get; set; }
    
    public int? TurnNum { get; set; }
    
    public Guid? PirateId { get; set; }
}