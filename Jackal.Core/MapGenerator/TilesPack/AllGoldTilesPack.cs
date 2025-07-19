using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Набор только из клеток с монетами
/// </summary>
public class AllGoldTilesPack : ITilesPack
{
    /// <summary>
    /// 117 клеток
    /// </summary>
    public TileParams[] AllTiles { get; }
    
    public AllGoldTilesPack()
    {
        AllTiles = new TileParams[117];
        for (var index = 0; index < AllTiles.Length; index++)
        {
            AllTiles[index] = index % 2 == 0 
                ? TileParams.BigCoin() 
                : TileParams.Coin();
        }
    }
}