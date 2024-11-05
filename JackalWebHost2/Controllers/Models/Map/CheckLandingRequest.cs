namespace JackalWebHost2.Controllers.Models.Map;

/// <summary>
/// Запрос для проверки места высадки
/// </summary>
public class CheckLandingRequest
{
    /// <summary>
    /// ИД карты, по нему генерируется расположение клеток
    /// </summary>
    public int MapId { get; set; }
        
    /// <summary>
    /// Размер стороны карты с учетом воды
    /// </summary>
    public int MapSize { get; set; }

    /// <summary>
    /// Название игрового набора клеток
    /// </summary>
    public string? TilesPackName { get; set; }
}