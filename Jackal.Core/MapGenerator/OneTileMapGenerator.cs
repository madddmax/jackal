namespace Jackal.Core;

/// <summary>
/// Все клетки oneTileParams
/// </summary>
public class OneTileMapGenerator(TileParams oneTileParams) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator = 
        new(oneTileParams, oneTileParams, oneTileParams);

    public int MapId => _mapGenerator.MapId;

    public int CoinsOnMap => _mapGenerator.CoinsOnMap;

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);
}