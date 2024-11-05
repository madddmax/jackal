namespace JackalWebHost2.Controllers.Models.Map;

/// <summary>
/// Результат проверки места высадки
/// </summary>
public class CheckLandingResponse(short danger, short wealth)
{
    /// <summary>
    /// Опасность
    /// </summary>
    public short Danger { get; set; } = danger;

    /// <summary>
    /// Богатство
    /// </summary>
    public short Wealth { get; set; } = wealth;
}