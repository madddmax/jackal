namespace Jackal.Core;

/// <summary>
/// Нижняя береговая линия firstTile,
/// остальные все клетки secondTile
/// </summary>
public class TwoTileMapGenerator(TileParams firstTileParams, TileParams secondTileParams) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator = 
        new(firstTileParams, secondTileParams, secondTileParams);

    public int MapId => _mapGenerator.MapId;

    public int CoinsOnMap => _mapGenerator.CoinsOnMap;

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);
}