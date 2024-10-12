namespace Jackal.Core.MapGenerator;

/// <summary>
/// Нижняя береговая линия firstTile,
/// остальные все клетки secondTile
/// </summary>
public class TwoTileMapGenerator(TileParams firstTileParams, TileParams secondTileParams, int coinsOnMap = 0) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator = 
        new(firstTileParams, secondTileParams, secondTileParams);

    /// <summary>
    /// Идентификатор карты
    /// </summary>
    public int MapId => _mapGenerator.MapId;

    /// <summary>
    /// Монет на карте, нужно сразу рассчитать т.к. используется при инициализации Game
    /// </summary>
    public int CoinsOnMap => coinsOnMap;

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);
    
    public void Swap(Position from, Position to) => _mapGenerator.Swap(from, to);
}