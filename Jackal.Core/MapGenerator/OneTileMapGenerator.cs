using System.Collections.Generic;

namespace Jackal.Core;

public class OneTileMapGenerator(TileParams tileParams) : IMapGenerator
{
    private readonly TileParams _tileParams = tileParams;

    private readonly Dictionary<Position, Tile> _tiles = new();
    
    public int MapId { get; } = 0;

    public int CoinsOnMap { get; } = 0;

    public Tile GetNext(Position position)
    {
        if (!_tiles.ContainsKey(position))
        {
            _tileParams.Position = position;
            _tiles[position] = new Tile(_tileParams);
        }
        
        return _tiles[position];
    }
}