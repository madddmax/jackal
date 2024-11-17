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
        new TileParams(TileType.Chest1), // 1 монета - первый сундук берем всегда
        new TileParams(TileType.Chest1), // 2
        new TileParams(TileType.Chest1), // 3
        new TileParams(TileType.Chest1), // 4
        new TileParams(TileType.Chest1), // 5
        new TileParams(TileType.Chest1), // 6
        new TileParams(TileType.Chest1), // 7
        new TileParams(TileType.Chest1), // 8
        new TileParams(TileType.Chest2), // 10
        new TileParams(TileType.Chest2), // 12
        new TileParams(TileType.Chest2), // 14
        new TileParams(TileType.Chest2), // 16
        new TileParams(TileType.Chest3), // 19
        new TileParams(TileType.Chest3), // 21
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