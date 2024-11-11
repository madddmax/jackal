using Jackal.Core.Domain;

namespace JackalWebHost2.Models.Map;

/// <summary>
/// Результат проверки места высадки
/// </summary>
public class CheckLandingResult(DirectionType direction)
{
    /// <summary>
    /// Направление
    /// </summary>
    public DirectionType Direction { get; } = direction;
    
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