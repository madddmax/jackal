namespace Jackal.Core.Players;

/// <summary>
/// Статистика игрока
/// </summary>
public class GamePlayerStat
{
    /// <summary>
    /// Имя пользователя или бота
    /// </summary>
    public string PlayerName { get; set; }
    
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
    /// Количество проведенных игр за день
    /// </summary>
    public int GamesCountToday { get; set; }
    
    /// <summary>
    /// Количество проведенных игр за неделю
    /// </summary>
    public int GamesCountThisWeek { get; set; }
    
    /// <summary>
    /// Количество проведенных игр за месяц
    /// </summary>
    public int GamesCountThisMonth { get; set; }
    
    /// <summary>
    /// Количество проведенных игр за всё время
    /// </summary>
    public int GamesCountTotal { get; set; }
    
    /// <summary>
    /// Среднее количество побед за все игры
    /// </summary>
    public double AverageWin => (double)TotalWin / GamesCountTotal;
        
    /// <summary>
    /// Среднее количество добытых монет за все игры
    /// </summary>
    public double AverageCoins => (double)TotalCoins / GamesCountTotal;
}