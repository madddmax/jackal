namespace JackalWebHost2.Controllers.Models.Leaderboard;

/// <summary>
/// Сортировка лидеров
/// </summary>
public enum LeaderboardOrderByType
{
    /// <summary>
    /// По добытым монетам
    /// </summary>
    TotalCoins = 0,
    
    /// <summary>
    /// По победам
    /// </summary>
    TotalWin = 1,
    
    /// <summary>
    /// По играм
    /// </summary>
    GamesCount = 2
}