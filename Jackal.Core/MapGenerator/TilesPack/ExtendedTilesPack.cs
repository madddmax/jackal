using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Расширенный игровой набор
/// </summary>
public class ExtendedTilesPack : ITilesPack
{
    /// <summary>
    /// 120 клеток
    /// </summary>
    public TileParams[] AllTiles { get; } =
    [
        // 96 значимых клеток
        new TileParams(TileType.Chest1), // 1 монета - первый сундук берем всегда
        new TileParams(TileType.Chest1), // 2
        new TileParams(TileType.Chest1), // 3
        new TileParams(TileType.Chest1), // 4
        new TileParams(TileType.Chest1), // 5
        new TileParams(TileType.Chest2), // 7
        new TileParams(TileType.Chest2), // 9
        new TileParams(TileType.Chest2), // 11
        new TileParams(TileType.Chest2), // 13
        new TileParams(TileType.Chest3), // 16
        new TileParams(TileType.Chest3), // 19
        new TileParams(TileType.Chest3), // 21
        new TileParams(TileType.Chest4), // 25
        new TileParams(TileType.Chest4), // 29
        new TileParams(TileType.Chest5), // 34
        new TileParams(TileType.Fort),
        new TileParams(TileType.Fort),
        new TileParams(TileType.RespawnFort), // порядок RespawnFort и Cannibal важен для баланса
        new TileParams(TileType.Cannibal), // берем воскрешающий форт вместе с людоедом
        new TileParams(TileType.RespawnFort),
        new TileParams(TileType.Cannibal),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Airplane),
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
        new TileParams(TileType.Lighthouse),
        new TileParams(TileType.Lighthouse),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
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
        new TileParams(TileType.Spinning) { SpinningCount = 4 },
        new TileParams(TileType.Spinning) { SpinningCount = 5 },
        new TileParams(TileType.Spinning) { SpinningCount = 5 },
        new TileParams(TileType.Caramba),
        new TileParams(TileType.Jungle),
        new TileParams(TileType.Jungle),
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        // 24 пустых клеток
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
        new TileParams(TileType.Grass, 3)
    ];
}