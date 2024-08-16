using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Нижняя береговая линия - золото,
/// остальные все клетки людоеды
/// </summary>
public class GoldAndCannibalMapGenerator : IMapGenerator
{
    private readonly Dictionary<Position, Tile> _tiles = new();
    
    public int MapId => 0;

    public int CoinsOnMap { get; private set; } = 0;

    public Tile GetNext(Position position)
    {
        if (!_tiles.ContainsKey(position))
        {
            var tileParams = new TileParams
            {
                Position = position,
                Type = position.Y == 1 ? TileType.Chest1 : TileType.Cannibal,
            };
            
            var tile = new Tile(tileParams);
            if (tile.Type.CoinsCount() > 0)
            {
                tile.Levels[0].Coins = tile.Type.CoinsCount();
                CoinsOnMap += tile.Type.CoinsCount();
            }

            _tiles[position] = tile;
        }
        
        return _tiles[position];
    }
}