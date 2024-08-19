using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Нижняя береговая линия firstTile,
/// следующая линия secondTile,
/// остальные все клетки thirdTile
/// </summary>
public class ThreeTileMapGenerator(
    TileParams firstTileParams, TileParams secondTileParams, TileParams thirdTileParams) : IMapGenerator
{
    private readonly Dictionary<Position, Tile> _tiles = new();
    
    public int MapId => 0;

    public int CoinsOnMap { get; private set; } = 0;

    public Tile GetNext(Position position)
    {
        if (!_tiles.ContainsKey(position))
        {
            var tileParams = position.Y switch
            {
                1 => firstTileParams,
                2 => secondTileParams,
                _ => thirdTileParams
            };

            tileParams.Position = position;
            
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