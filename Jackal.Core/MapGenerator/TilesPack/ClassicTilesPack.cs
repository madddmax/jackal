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
        new TileParams(TileType.Chest1), // 1 монета - первый сундук берем всегда
        new TileParams(TileType.Chest1), // 2
        new TileParams(TileType.Chest1), // 3
        new TileParams(TileType.Chest1), // 4
        new TileParams(TileType.Chest1), // 5
        new TileParams(TileType.Chest2), // 7
        new TileParams(TileType.Chest2), // 9
        new TileParams(TileType.Chest2), // 11
        new TileParams(TileType.Chest2), // 13
        new TileParams(TileType.Chest2), // 15
        new TileParams(TileType.Chest3), // 18
        new TileParams(TileType.Chest3), // 21
        new TileParams(TileType.Chest3), // 24
        new TileParams(TileType.Chest4), // 28
        new TileParams(TileType.Chest4), // 32
        new TileParams(TileType.Chest5), // 37
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