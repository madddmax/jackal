namespace JackalWebHost2.Models;

/// <summary>
/// Текущая статистика игры
/// </summary>
public class GameStatistics
{
    /// <summary>
    /// Номер хода
    /// </summary>
    public int TurnNumber;
    
    /// <summary>
    /// Конец игры
    /// </summary>
    public bool IsGameOver;
    
    /// <summary>
    /// Игровое сообщение
    /// </summary>
    public string GameMessage;
    
    /// <summary>
    /// ИД команды пиратов чей ход
    /// </summary>
    public int CurrentTeamId;

    /// <summary>
    /// ИД игрока чей ход,
    /// отличается от ИД команды при розыгрыше хи-хи травы
    /// </summary>
    public int CurrentPlayerId;
}