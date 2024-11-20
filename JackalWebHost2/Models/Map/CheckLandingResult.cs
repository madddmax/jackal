namespace JackalWebHost2.Models.Map;

/// <summary>
/// Результат проверки места высадки
/// </summary>
public class CheckLandingResult(MapPositionId position)
{
    /// <summary>
    /// Позиция
    /// </summary>
    public MapPositionId Position { get; } = position;
    
    /// <summary>
    /// Сложность
    /// </summary>
    public DifficultyLevel Difficulty { get; set; }
    
    /// <summary>
    /// Золото
    /// </summary>
    public int Coins { get; set; }
    
    /// <summary>
    /// Людоеды
    /// </summary>
    public int Cannibals { get; set; }
}