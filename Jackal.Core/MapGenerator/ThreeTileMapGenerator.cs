using System.Collections.Generic;

namespace Jackal.Core.MapGenerator;

/// <summary>
/// Нижняя береговая линия firstTile,
/// следующая линия secondTile,
/// остальные все клетки thirdTile
/// </summary>
public class ThreeTileMapGenerator(
    TileParams firstTileParams, TileParams secondTileParams, TileParams thirdTileParams, int coinsOnMap = 0) : IMapGenerator
{
    private readonly Dictionary<Position, Tile> _tiles = new();
    
    /// <summary>
    /// Идентификатор карты
    /// </summary>
    public int MapId => 0;

    /// <summary>
    /// Монет на карте, нужно сразу рассчитать т.к. используется при инициализации Game
    /// </summary>
    public int CoinsOnMap { get; } = coinsOnMap;
    
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
            }

            _tiles[position] = tile;
        }
        
        return _tiles[position];
    }

    public void Swap(Position from, Position to)
    {
        // инициализируем клетки, если их нет
        GetNext(from);
        GetNext(to);
        
        // меняем сгенеренные клетки местами
        var fromTile = _tiles[from];
        var toTile = _tiles[to];
        
        _tiles[from] = new Tile(from, toTile);
        _tiles[from].Levels[0].Coins = toTile.Coins;
        
        _tiles[to] = new Tile(to, fromTile);
        _tiles[to].Levels[0].Coins = fromTile.Coins;
    }
}