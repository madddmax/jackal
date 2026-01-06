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
    /// Количество побед за день
    /// </summary>
    public int WinCountToday { get; set; }
    
    /// <summary>
    /// Количество побед за неделю
    /// </summary>
    public int WinCountThisWeek { get; set; }
    
    /// <summary>
    /// Количество побед за месяц
    /// </summary>
    public int WinCountThisMonth { get; set; }
    
    /// <summary>
    /// Количество побед, когда в конце игры золота оказалось больше.
    /// В случае равенства по золоту, победа присуждается обеим командам.
    /// </summary>
    public int TotalWin { get; set; }

    /// <summary>
    /// Ранг по шкале силы юнитов игры Heroes of Might and Magic II
    /// </summary>
    public string Rank => TotalWin.GetHeroes2Rank();

    /// <summary>
    /// Количество поражений за день
    /// </summary>
    public int LoseCountToday { get; set; }
    
    /// <summary>
    /// Количество поражений за неделю
    /// </summary>
    public int LoseCountThisWeek { get; set; }
    
    /// <summary>
    /// Количество поражений за месяц
    /// </summary>
    public int LoseCountThisMonth { get; set; }
    
    /// <summary>
    /// Количество поражений за всё время
    /// </summary>
    public int TotalLose { get; set; }
    
    /// <summary>
    /// Суммарное количество добытых монет за все игры
    /// </summary>
    public int TotalCoins { get; set; }
    
    /// <summary>
    /// Процент побед за все игры
    /// </summary>
    public double WinPercent => (double)TotalWin * 100 / (TotalWin + TotalLose);
        
    /// <summary>
    /// Среднее количество добытых монет за все игры
    /// </summary>
    public double AverageCoins => (double)TotalCoins / (TotalWin + TotalLose);
}