namespace JackalWebHost2.Data.Entities;

public class UserEntity
{
    /// <summary>
    /// ИД пользователя
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Login { get; set; }
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime Created { get; set; }
    
    public virtual List<GameEntity> Games { get; set; }
    
    public virtual List<GamePlayerEntity> GamePlayers { get; set; }
}