namespace Jackal.BotArena;

/// <summary>
/// Статистика игрока
/// </summary>
public class GamePlayerStat
{
    /// <summary>
    /// Количество побед, когда в конце игры золота оказалось больше.
    /// В случае равенства по золоту, победа присуждается обеим командам.
    /// </summary>
    public int TotalWin { get; set; }
        
    /// <summary>
    /// Суммарное количество добытых монет за все игры
    /// </summary>
    public int TotalCoins { get; set; }
        
    /// <summary>
    /// Количество проведенных игр
    /// </summary>
    public int GamesCount { get; set; }

    /// <summary>
    /// Среднее количество побед за все игры
    /// </summary>
    public double AverageWin => (double)TotalWin / GamesCount;
        
    /// <summary>
    /// Среднее количество добытых монет за все игры
    /// </summary>
    public double AverageCoins => (double)TotalCoins / GamesCount;
    
    /// <summary>
    /// Количество совершённых ходов
    /// </summary>
    public int TotalTurns { get; set; }
    
    /// <summary>
    /// Среднее количество ходов
    /// </summary>
    public double AverageTurns => (double)TotalTurns / GamesCount;
}