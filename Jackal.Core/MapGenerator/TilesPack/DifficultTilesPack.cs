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
        TileFactory.Cannibal(), // берем сундук вместе с людоедом
        TileFactory.Cannibal(),
        TileFactory.Cannibal(),
        TileFactory.Cannibal(),
        TileFactory.Cannibal(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.Horse(),
        TileFactory.Horse(),
        TileFactory.Horse(),
        TileFactory.Horse(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Airplane(),
        TileFactory.Crocodile(),
        TileFactory.Crocodile(),
        TileFactory.Crocodile(),
        TileFactory.Crocodile(),
        TileFactory.Ice(),
        TileFactory.Ice(),
        TileFactory.Ice(),
        TileFactory.Ice(),
        TileFactory.Ice(),
        TileFactory.Ice(),
        TileFactory.Cannon(),
        TileFactory.Cannon(),
        TileFactory.Cannon(),
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
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
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