namespace JackalWebHost2.Data.Entities;

public class GamePlayerEntity
{
    public long Id { get; set; }
    
    public long GameId { get; set; }
    
    public long? UserId { get; set; }
    
    public string PlayerName { get; set; }
    
    public byte MapPositionId { get; set; }
    
    public virtual GameEntity Game { get; set; }
    
    public virtual UserEntity User { get; set; }
}