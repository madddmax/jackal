using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Интерфейс игрового набора. Минимум 117 клеток,
/// самая большая карта 13x13 = 117 клеток
/// </summary>
public interface ITilesPack
{
    /// <summary>
    /// Состав клеток в наборе
    /// </summary>
    TileParams[] AllTiles { get; }
}