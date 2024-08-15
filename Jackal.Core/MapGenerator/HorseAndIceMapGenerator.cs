using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Нижняя береговая линия - конь,
/// остальные все клетки лед
/// </summary>
public class HorseAndIceMapGenerator() : IMapGenerator
{
    private readonly Dictionary<Position, Tile> _tiles = new();
    
    public int MapId { get; } = 0;

    public int CoinsOnMap { get; } = 0;

    public Tile GetNext(Position position)
    {
        if (!_tiles.ContainsKey(position))
        {
            var tileParams = new TileParams
            {
                Position = position,
                Type = position.Y == 1 ? TileType.Horse : TileType.Ice
            };
            _tiles[position] = new Tile(tileParams);
        }
        
        return _tiles[position];
    }
}