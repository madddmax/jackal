namespace Jackal.Core.MapGenerator;

public interface IMapGenerator
{
    /// <summary>
    /// Идентификатор карты
    /// </summary>
    public int MapId { get; }
    
    /// <summary>
    /// Монет на карте, нужно сразу рассчитать т.к. используется при инициализации Game
    /// </summary>
    int CoinsOnMap { get; }

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