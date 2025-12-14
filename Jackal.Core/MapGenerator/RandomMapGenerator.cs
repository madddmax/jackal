using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core.Domain;
using Jackal.Core.MapGenerator.TilesPack;

namespace Jackal.Core.MapGenerator;

public class RandomMapGenerator : IMapGenerator
{
    private readonly Random _rand;
    private readonly Dictionary<Position,Tile> _tiles;

    public int MapId { get; }
    
    public string TilesPackName { get; }
    
    public int TotalCoins { get; private set; }
    
    public RandomMapGenerator(int mapId, int mapSize, string? tilesPackName = null)
    {
        MapId = mapId;
        _rand = new Random(mapId + 5000000);
        
        TilesPackName = TilesPackFactory.CheckName(tilesPackName);
        var pack = TilesPackFactory.Create(tilesPackName);
        
        var selectedTiles = PullOut(_rand, mapSize, pack);
        var shuffledTiles = Shuffle(selectedTiles);
        
        var positions = GetAllEarth(mapSize).ToList();

        if (shuffledTiles.Count != positions.Count)
            throw new Exception("Wrong tiles pack count");

        _tiles = InitTiles(shuffledTiles, positions);
    }

    private List<TileParams> PullOut(Random rand, int mapSize, ITilesPack pack)
    {
        var landSize = mapSize - 2;
        var totalTiles = landSize * landSize - 4;
        var list = new List<TileParams>(totalTiles);

        // выбираем обязательный сундук с 1 монетой
        bool random = false;
        int selectedIndex = 0;
            
        for (var i = 0; i < totalTiles; i++)
        {
            var index = random 
                ? rand.Next(0, pack.AllTiles.Length - i) 
                : selectedIndex;
                
            list.Add(pack.AllTiles[index]);

            switch (pack.AllTiles[index].Type)
            {
                case TileType.Cannibal:
                    // выбираем воскрешающий форт к людоеду
                    random = false;
                    selectedIndex = index > 0 ? index - 1 : 0;
                    break;
                case TileType.RespawnFort:
                    // выбираем людоеда к воскрешающему форту
                    random = false;
                    selectedIndex = index + 1;
                    break;
                default:
                    random = true;
                    break;
            }

            var tileParam = pack.AllTiles[index];
            TotalCoins += tileParam.Type == TileType.Coin ? tileParam.Code : 0;
            TotalCoins += tileParam.Type == TileType.BigCoin ? tileParam.Code * Constants.BigCoinValue : 0;

            // сдвигаем оставшиеся клетки в наборе, последнюю ставим на место выбранной
            pack.AllTiles[index] = pack.AllTiles[pack.AllTiles.Length - 1 - i];
        }

        // если сгенерилась одна дыра на карту - то заменяем её на пустую клетку
        var holeTilesCount = list.Count(x => x.Type == TileType.Hole);
        if (holeTilesCount == 1)
        {
            var hole = list.First(x => x.Type == TileType.Hole);
            list.Remove(hole);
            list.Add(TileParams.Empty());
        }

        return list;
    }
    
    private List<TileParams> Shuffle(IEnumerable<TileParams> defs)
    {
        return defs
            .Select(x => new {Def = x, Number = _rand.NextDouble()})
            .OrderBy(x => x.Number)
            .Select(x => x.Def)
            .ToList();
    }
    
    private static IEnumerable<Position> GetAllEarth(int mapSize)
    {
        for (int x = 1; x <= mapSize - 2; x++)
        {
            for (int y = 1; y <= mapSize - 2; y++)
            {
                Position val = new Position(x, y);
                if (Utils.InCorners(val, 1, mapSize - 2) == false)
                {
                    yield return val;
                }
            }
        }
    }
    
    private Dictionary<Position, Tile> InitTiles(List<TileParams> pack, List<Position> positions)
    {
        var tiles = new Dictionary<Position, Tile>();

        foreach (var info in pack.Zip(positions, (def, position) => new {Def = def, Position = position}))
        {
            var tempDef = info.Def.Clone();
            if (tempDef.Type != TileType.Spinning && 
                tempDef.Type != TileType.Caramba &&
                tempDef.Type != TileType.Coin &&
                tempDef.Type != TileType.BigCoin &&
                tempDef.Type != TileType.Ice &&
                tempDef.Type != TileType.Hole)
            {
                // клетки не указанные в условии - вращаем при отображении на карте
                tempDef.Direction = (DirectionType)_rand.Next(4);
            }
                
            if (tempDef.Type is TileType.Arrow)
            {
                for (var j = 1; j <= (int)tempDef.Direction; j++)
                {
                    tempDef.Code = ArrowsCodesHelper.DoRotate(tempDef.Code);
                }
            }

            tempDef.Position = info.Position;
                
            var tile = new Tile(tempDef);
            tile.Levels[0].Coins = tile.CoinsCount();
            tile.Levels[0].BigCoins = tile.BigCoinsCount();
                
            tiles.Add(info.Position, tile);
        }

        return tiles;
    }
        
    public Tile GetNext(Position position) => _tiles[position];

    public void Swap(Position from, Position to)
    {
        // меняем сгенеренные клетки местами
        var fromTile = _tiles[from];
        var toTile = _tiles[to];
        
        _tiles[from] = new Tile(from, toTile);
        _tiles[from].Levels[0].Coins = toTile.Coins;
        _tiles[from].Levels[0].BigCoins = toTile.BigCoins;
        
        _tiles[to] = new Tile(to, fromTile);
        _tiles[to].Levels[0].Coins = fromTile.Coins;
        _tiles[to].Levels[0].BigCoins = fromTile.BigCoins;
    }
}