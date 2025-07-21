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
    /// ИД команды которая ходит
    /// </summary>
    public int CurrentTeamId;
    
    /// <summary>
    /// Ходы за другую команду при розыгрыше хи-хи травы
    /// </summary>
    public bool WithCannabis;
}