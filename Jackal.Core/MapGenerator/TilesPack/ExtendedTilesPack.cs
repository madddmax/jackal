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
        // 104 значимых клеток
        TileFactory.Coin(), // 1 монета - первый сундук берем всегда
        TileFactory.Coin(), // 2
        TileFactory.Coin(), // 3
        TileFactory.Coin(), // 4
        TileFactory.Coin(), // 5
        TileFactory.Coin(2), // 7
        TileFactory.Coin(2), // 9
        TileFactory.Coin(2), // 11
        TileFactory.Coin(2), // 13
        TileFactory.Coin(3), // 16
        TileFactory.Coin(3), // 19
        TileFactory.Coin(3), // 21
        TileFactory.Coin(4), // 25
        TileFactory.Coin(4), // 29
        TileFactory.Coin(5), // 34
        TileFactory.BigCoin(), // 37
        TileFactory.BigCoin(), // 40
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
        new TileParams(TileType.RumBottles, 1),
        new TileParams(TileType.RumBottles, 1),
        new TileParams(TileType.RumBottles, 1),
        new TileParams(TileType.RumBottles, 2),
        new TileParams(TileType.RumBottles, 2),
        new TileParams(TileType.RumBottles, 3),
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
        // 16 пустых клеток
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 0),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 1),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 2),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3),
        new TileParams(TileType.Grass, 3)
    ];
}