using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Все клетки oneTileParams
/// </summary>
public class OneTileMapGenerator(TileParams oneTileParams) : IMapGenerator
{
    private readonly Dictionary<Position, Tile> _tiles = new();
    
    public int MapId { get; } = 0;

    public int CoinsOnMap { get; } = 0;

    public Tile GetNext(Position position)
    {
        if (!_tiles.ContainsKey(position))
        {
            oneTileParams.Position = position;
            _tiles[position] = new Tile(oneTileParams);
        }
        
        return _tiles[position];
    }
}