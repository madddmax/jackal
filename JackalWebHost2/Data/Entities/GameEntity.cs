using Jackal.Core;

namespace JackalWebHost2.Data.Entities;

public class GameEntity
{
    /// <summary>
    /// ИД игры
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// ИД карты, по нему генерируется расположение клеток
    /// </summary>
    public int MapId { get; set; }
    
    /// <summary>
    /// Название игрового набора клеток
    /// </summary>
    public string TilesPackName { get; set; }
    
    /// <summary>
    /// Размер стороны карты с учетом воды
    /// </summary>
    public int MapSize { get; set; }
    
    /// <summary>
    /// Режим игры
    /// </summary>
    public GameModeType GameMode { get; set; }
    
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
    
    /// <summary>
    /// Игра завершена
    /// </summary>
    public bool GameOver { get; set; }
    
    public virtual UserEntity CreatorUser { get; set; }
    
    public virtual List<GamePlayerEntity> GamePlayers { get; set; }
}