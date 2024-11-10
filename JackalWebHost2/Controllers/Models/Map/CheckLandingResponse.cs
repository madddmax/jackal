using Jackal.Core.Domain;

namespace JackalWebHost2.Controllers.Models.Map;

/// <summary>
/// Результат проверки места высадки
/// </summary>
public class CheckLandingResponse(DirectionType direction)
{
    /// <summary>
    /// Направление
    /// </summary>
    public DirectionType Direction { get; } = direction;
    
    /// <summary>
    /// Опасность
    /// </summary>
    public int Danger { get; set; }

    /// <summary>
    /// Богатство
    /// </summary>
    public int Wealth { get; set; }
}