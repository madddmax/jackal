using JackalWebHost2.Models.Player;

namespace JackalWebHost2.Models;

public class GameSettings
{
    /// <summary>
    /// Игроки robot/human
    /// </summary>
    public Player[] Players { get; set; } = null!;
        
    /// <summary>
    /// Игроки - новая модель,
    /// todo заменить Players
    /// </summary>
    public PlayerModel[] PlayersNew { get; set; } = null!;
    
    /// <summary>
    /// ИД карты, по нему генерируется расположение клеток
    /// </summary>
    public int? MapId { get; set; }
        
    /// <summary>
    /// Размер стороны карты с учетом воды
    /// </summary>
    public int? MapSize { get; set; }
    
    /// <summary>
    /// Название игрового набора клеток
    /// </summary>
    public string? TilesPackName { get; set; }
    
    /// <summary>
    /// Режим игры
    /// todo добавить командный режим 2x2 
    /// </summary>
    public GameModeType? Mode { get; set; }
}