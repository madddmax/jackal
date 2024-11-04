using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Набор из одной монеты и толпы Бен Ганнов
/// </summary>
public class LotsOfBenTilesPack : ITilesPack
{
    /// <summary>
    /// 117 клеток
    /// </summary>
    public TileParams[] AllTiles { get; }
    
    public LotsOfBenTilesPack()
    {
        AllTiles = new TileParams[117];
        AllTiles[0] = new TileParams(TileType.Chest1);
        for (var index = 1; index < AllTiles.Length; index++)
        {
            AllTiles[index] = new TileParams(TileType.BenGunn);
        }
    }
}