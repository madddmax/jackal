using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator;

/// <summary>
/// Все клетки oneTileParams
/// </summary>
public class OneTileMapGenerator(TileParams oneTileParams) : IMapGenerator
{
    private readonly ThreeTileMapGenerator _mapGenerator = 
        new(oneTileParams, oneTileParams, oneTileParams);

    /// <summary>
    /// Идентификатор карты
    /// </summary>
    public int MapId => _mapGenerator.MapId;

    public Tile GetNext(Position position) => _mapGenerator.GetNext(position);
    
    public void Swap(Position from, Position to) => _mapGenerator.Swap(from, to);
}