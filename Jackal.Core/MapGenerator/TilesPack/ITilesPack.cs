using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Интерфейс игрового набора. Минимум 120 клеток,
/// самая большая карта 13x13 имеет 117 клеток
/// </summary>
public interface ITilesPack
{
    /// <summary>
    /// Название набора
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Состав клеток в наборе
    /// </summary>
    TileParams[] AllTiles { get; }
}