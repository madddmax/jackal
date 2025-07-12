using Jackal.Core.Domain;

namespace Jackal.Core;

public static class TileExtension
{
    public static int CoinsCount(this Tile tile) =>
        tile.Type == TileType.Coin ? tile.Code : 0;
    
    public static int BigCoinsCount(this Tile tile) =>
        tile.Type == TileType.BigCoin ? tile.Code : 0;
}