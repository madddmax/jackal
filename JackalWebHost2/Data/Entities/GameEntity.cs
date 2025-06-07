namespace JackalWebHost2.Data.Entities;

public class GameEntity
{
    /// <summary>
    /// ИД игры
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// ИД пользователя создателя игры
    /// </summary>
    public long CreatorUserId { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime Created { get; set; }
    
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime Updated { get; set; }
    
    /// <summary>
    /// Номер хода
    /// </summary>
    public int TurnNumber { get; set; }
    
    public virtual UserEntity CreatorUser { get; set; }
    
    public virtual List<GamePlayerEntity> GamePlayers { get; set; }
}