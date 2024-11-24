using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator;

/// <summary>
/// Нижняя береговая линия firstTile,
/// остальные все клетки secondTile
/// </summary>
public class TwoTileMapGenerator(TileParams firstTileParams, TileParams secondTileParams) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator = 
        new(firstTileParams, secondTileParams, secondTileParams);

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);
    
    public void Swap(Position from, Position to) => _mapGenerator.Swap(from, to);
}