using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator;

public interface IMapGenerator
{
    /// <summary>
    /// ИД карты, по нему генерируется расположение клеток
    /// </summary>
    public int MapId { get; }

    /// <summary>
    /// Название игрового набора клеток
    /// </summary>
    public string TilesPackName { get; }
    
    /// <summary>
    /// Всего золота на карте
    /// </summary>
    int TotalCoins { get; }
    
    /// <summary>
    /// Открыть закрытую клетку,
    /// тип клетки строго привязан к позиции на карте,
    /// сделано для возможности воспроизведения карты
    /// </summary>
    Tile GetNext(Position position);

    /// <summary>
    /// Поменять клетки в генераторе местами,
    /// требуется для Разлома, чтобы не множить одинаковые клетки
    /// </summary>
    void Swap(Position from, Position to);
}