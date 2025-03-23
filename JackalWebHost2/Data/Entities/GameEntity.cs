namespace JackalWebHost2.Data.Entities;

public class GameEntity
{
    public long Id { get; set; }
    
    public string Code { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Updated { get; set; }
    
    public int TurnNumber { get; set; }
    
    public virtual List<GameUserEntity> GameUsers { get; set; }
}