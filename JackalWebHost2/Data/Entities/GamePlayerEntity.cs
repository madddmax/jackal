namespace JackalWebHost2.Data.Entities;

public class GamePlayerEntity
{
    /// <summary>
    /// ИД сущности
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// ИД игры
    /// </summary>
    public long GameId { get; set; }
    
    /// <summary>
    /// ИД команды, уникально в рамках игры
    /// </summary>
    public int TeamId { get; set; }
    
    /// <summary>
    /// ИД пользователя, если бот то null
    /// </summary>
    public long? UserId { get; set; }
    
    /// <summary>
    /// Имя пользователя или бота
    /// </summary>
    public string PlayerName { get; set; }
    
    /// <summary>
    /// ИД позиции относительно игровой карты
    /// </summary>
    public byte MapPositionId { get; set; }
    
    /// <summary>
    /// Монеты на корабле
    /// </summary>
    public int Coins { get; set; }
    
    /// <summary>
    /// Победитель игры
    /// </summary>
    public bool Winner { get; set; }
    
    public virtual GameEntity Game { get; set; }
    
    public virtual UserEntity User { get; set; }
}