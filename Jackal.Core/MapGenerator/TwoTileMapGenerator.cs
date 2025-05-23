using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator;

/// <summary>
/// Нижняя береговая линия firstTile,
/// остальные все клетки secondTile
/// </summary>
public class TwoTileMapGenerator(
    TileParams firstTileParams,
    TileParams secondTileParams,
    int totalCoins = 1
) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator =
        new(firstTileParams, secondTileParams, secondTileParams, totalCoins);

    public int MapId => _mapGenerator.MapId;
    
    public string TilesPackName => _mapGenerator.TilesPackName;
    
    public int TotalCoins => _mapGenerator.TotalCoins;

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);

    public void Swap(Position from, Position to) => _mapGenerator.Swap(from, to);
}