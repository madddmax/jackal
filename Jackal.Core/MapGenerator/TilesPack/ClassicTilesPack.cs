using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Классический игровой набор
/// </summary>
public class ClassicTilesPack : ITilesPack
{
    /// <summary>
    /// 117 клеток
    /// </summary>
    public TileParams[] AllTiles { get; } =
    [
        // 77 значимых клеток
        TileFactory.Coin(), // 1 монета - первый сундук берем всегда
        TileFactory.Coin(), // 2
        TileFactory.Coin(), // 3
        TileFactory.Coin(), // 4
        TileFactory.Coin(), // 5
        TileFactory.Coin(2), // 7
        TileFactory.Coin(2), // 9
        TileFactory.Coin(2), // 11
        TileFactory.Coin(2), // 13
        TileFactory.Coin(2), // 15
        TileFactory.Coin(3), // 18
        TileFactory.Coin(3), // 21
        TileFactory.Coin(3), // 24
        TileFactory.Coin(4), // 28
        TileFactory.Coin(4), // 32
        TileFactory.Coin(5), // 37
        TileFactory.Fort(),
        TileFactory.Fort(),
        TileFactory.RespawnFort(), // порядок RespawnFort и Cannibal важен для баланса
        TileFactory.Cannibal(), // берем воскрешающий форт вместе с людоедом
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Crocodile),
        new TileParams(TileType.Crocodile),
        new TileParams(TileType.Crocodile),
        new TileParams(TileType.Crocodile),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Ice),
        new TileParams(TileType.Cannon),
        new TileParams(TileType.Cannon),
        TileFactory.OneArrowUp(),
        TileFactory.OneArrowUp(),
        TileFactory.OneArrowUp(),
        TileFactory.OneArrowDiagonal(),
        TileFactory.OneArrowDiagonal(),
        TileFactory.OneArrowDiagonal(),
        TileFactory.TwoArrowsDiagonal(),
        TileFactory.TwoArrowsDiagonal(),
        TileFactory.TwoArrowsDiagonal(),
        TileFactory.TwoArrowsLeftRight(),
        TileFactory.TwoArrowsLeftRight(),
        TileFactory.TwoArrowsLeftRight(),
        TileFactory.ThreeArrows(),
        TileFactory.ThreeArrows(),
        TileFactory.ThreeArrows(),
        TileFactory.FourArrowsPerpendicular(),
        TileFactory.FourArrowsPerpendicular(),
        TileFactory.FourArrowsPerpendicular(),
        TileFactory.FourArrowsDiagonal(),
        TileFactory.FourArrowsDiagonal(),
        TileFactory.FourArrowsDiagonal(),
        new TileParams(TileType.Trap),
        new TileParams(TileType.Trap),
        new TileParams(TileType.Trap),
        TileFactory.SpinningForest(),
        TileFactory.SpinningForest(),
        TileFactory.SpinningForest(),
        TileFactory.SpinningForest(),
        TileFactory.SpinningForest(),
        TileFactory.SpinningDesert(),
        TileFactory.SpinningDesert(),
        TileFactory.SpinningDesert(),
        TileFactory.SpinningDesert(),
        TileFactory.SpinningSwamp(),
        TileFactory.SpinningSwamp(),
        TileFactory.SpinningMount(),
        // 40 пустых клеток
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4), 
        TileFactory.Empty(4)
    ];
}