using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Безумный игровой набор от madddmax-а
/// </summary>
public class DifficultTilesPack : ITilesPack
{
    /// <summary>
    /// 120 клеток
    /// </summary>
    public TileParams[] AllTiles { get; } =
    [
        // 120 значимых клеток
        TileFactory.Coin(), // 1 монета - первый сундук берем всегда
        TileFactory.Coin(), // 2
        TileFactory.Coin(), // 3
        TileFactory.Coin(), // 4
        TileFactory.Coin(), // 5
        TileFactory.Coin(), // 6
        TileFactory.Coin(), // 7
        TileFactory.Coin(), // 8
        TileFactory.Coin(2), // 10
        TileFactory.Coin(2), // 12
        TileFactory.Coin(2), // 14
        TileFactory.Coin(2), // 16
        TileFactory.Coin(3), // 19
        TileFactory.Coin(3), // 21
        new TileParams(TileType.Cannibal), // берем сундук вместе с людоедом
        new TileParams(TileType.Cannibal),
        new TileParams(TileType.Cannibal),
        new TileParams(TileType.Cannibal),
        new TileParams(TileType.Cannibal),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.RumBarrel),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Horse),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Balloon),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
        new TileParams(TileType.Airplane),
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
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.BenGunn),
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Hole) { Direction = DirectionType.Left },
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake),
        new TileParams(TileType.Quake)
    ];
}