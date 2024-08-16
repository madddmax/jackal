using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Нижняя береговая линия - самолет,
/// остальные все клетки форт
/// </summary>
public class AirplaneAndFortMapGenerator() : IMapGenerator
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
                Type = position.Y == 1 ? TileType.Airplane : TileType.Fort
            };
            _tiles[position] = new Tile(tileParams);
        }
        
        return _tiles[position];
    }
}