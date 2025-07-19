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
        TileParams.Coin(), // 1 монета - первый сундук берем всегда
        TileParams.Coin(), // 2
        TileParams.Coin(), // 3
        TileParams.Coin(), // 4
        TileParams.Coin(), // 5
        TileParams.Coin(2), // 7
        TileParams.Coin(2), // 9
        TileParams.Coin(2), // 11
        TileParams.Coin(2), // 13
        TileParams.Coin(2), // 15
        TileParams.Coin(3), // 18
        TileParams.Coin(3), // 21
        TileParams.Coin(3), // 24
        TileParams.Coin(4), // 28
        TileParams.Coin(4), // 32
        TileParams.Coin(5), // 37
        TileParams.Fort(),
        TileParams.Fort(),
        TileParams.RespawnFort(), // порядок RespawnFort и Cannibal важен для баланса
        TileParams.Cannibal(), // берем воскрешающий форт вместе с людоедом
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Balloon(),
        TileParams.Balloon(),
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
        TileParams.Trap(),
        TileParams.Trap(),
        TileParams.Trap(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningSwamp(),
        TileParams.SpinningSwamp(),
        TileParams.SpinningMount(),
        // 40 пустых клеток
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4), 
        TileParams.Empty(4)
    ];
}