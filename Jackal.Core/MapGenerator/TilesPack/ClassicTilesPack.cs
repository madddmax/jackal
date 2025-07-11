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
        new TileParams(TileType.Fort),
        new TileParams(TileType.Fort),
        new TileParams(TileType.RespawnFort), // порядок RespawnFort и Cannibal важен для баланса
        new TileParams(TileType.Cannibal), // берем воскрешающий форт вместе с людоедом
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
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
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowUp),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowUp),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowUp),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.OneArrowDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsLeftRight),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsLeftRight),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.TwoArrowsLeftRight),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.ThreeArrows),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.ThreeArrows),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.ThreeArrows),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsPerpendicular),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsPerpendicular),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsPerpendicular),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsDiagonal),
        new TileParams(TileType.Arrow, ArrowsCodesHelper.FourArrowsDiagonal),
        new TileParams(TileType.Trap),
        new TileParams(TileType.Trap),
        new TileParams(TileType.Trap),
        new TileParams(TileType.Spinning) { SpinningCount = 2 },
        new TileParams(TileType.Spinning) { SpinningCount = 2 },
        new TileParams(TileType.Spinning) { SpinningCount = 2 },
        new TileParams(TileType.Spinning) { SpinningCount = 2 },
        new TileParams(TileType.Spinning) { SpinningCount = 2 },
        new TileParams(TileType.Spinning) { SpinningCount = 3 },
        new TileParams(TileType.Spinning) { SpinningCount = 3 },
        new TileParams(TileType.Spinning) { SpinningCount = 3 },
        new TileParams(TileType.Spinning) { SpinningCount = 3 },
        new TileParams(TileType.Spinning) { SpinningCount = 4 },
        new TileParams(TileType.Spinning) { SpinningCount = 4 },
        new TileParams(TileType.Spinning) { SpinningCount = 5 },
        // 40 пустых клеток
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3)
    ];
}