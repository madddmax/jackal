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
        TileParams.Coin(), // 1 монета - первый сундук берем всегда
        TileParams.Coin(), // 2
        TileParams.Coin(), // 3
        TileParams.Coin(), // 4
        TileParams.Coin(), // 5
        TileParams.Coin(), // 6
        TileParams.Coin(), // 7
        TileParams.Coin(), // 8
        TileParams.Coin(2), // 10
        TileParams.Coin(2), // 12
        TileParams.Coin(2), // 14
        TileParams.Coin(2), // 16
        TileParams.Coin(3), // 19
        TileParams.Coin(3), // 21
        TileParams.Cannibal(), // берем сундук вместе с людоедом
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Balloon(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Airplane(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Cannon(),
        TileParams.Cannon(),
        TileParams.Cannon(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Quake()
    ];
}