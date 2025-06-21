using Jackal.Core.Domain;

namespace Jackal.Core;

public static class TileTypeExtension
{
    public static int CoinsCount(this TileType source) =>
        source switch
        {
            TileType.Chest1 => 1,
            TileType.Chest2 => 2,
            TileType.Chest3 => 3,
            TileType.Chest4 => 4,
            TileType.Chest5 => 5,
            _ => 0
        };
    
    public static int BigCoinsCount(this TileType source) =>
        source switch
        {
            TileType.BigCoin => 1,
            _ => 0
        };
}