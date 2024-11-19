using Jackal.Core.Domain;
using JackalWebHost2.Models.Map;

namespace JackalWebHost2.Controllers.Models.Map;

/// <summary>
/// Результат проверки места высадки
/// </summary>
public class CheckLandingResponse
{
    /// <summary>
    /// Позиция
    /// </summary>
    public MapPositionId Position { get; set; }
    
    /// <summary>
    /// Сложность
    /// </summary>
    public DifficultyLevel Difficulty { get; set; }
}