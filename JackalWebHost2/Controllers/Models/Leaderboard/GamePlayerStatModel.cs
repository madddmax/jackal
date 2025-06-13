namespace JackalWebHost2.Controllers.Models.Leaderboard;

public class GamePlayerStatModel
{
    /// <summary>
    /// Номер позиции
    /// </summary>
    public int Number { get; set; }
    
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
    /// Суммарное количество добытых монет за выйгранные игры
    /// </summary>
    public int TotalWinCoins { get; set; }
}