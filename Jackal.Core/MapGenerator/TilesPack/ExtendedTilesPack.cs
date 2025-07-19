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
        TileFactory.Fort(),
        TileFactory.Fort(),
        TileFactory.RespawnFort(), // порядок RespawnFort и Cannibal важен для баланса
        TileFactory.Cannibal(), // берем воскрешающий форт вместе с людоедом
        TileFactory.RespawnFort(),
        TileFactory.Cannibal(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBarrel(),
        TileFactory.RumBottle(),
        TileFactory.RumBottle(),
        TileFactory.RumBottle(),
        TileFactory.RumBottle(2),
        TileFactory.RumBottle(2),
        TileFactory.RumBottle(3),
        TileFactory.Horse(),
        TileFactory.Horse(),
        TileFactory.Horse(),
        TileFactory.Balloon(),
        TileFactory.Balloon(),
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
        TileFactory.Trap(),
        TileFactory.Trap(),
        TileFactory.Trap(),
        TileFactory.Lighthouse(),
        TileFactory.Lighthouse(),
        TileFactory.BenGunn(),
        TileFactory.BenGunn(),
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
        TileFactory.SpinningSwamp(),
        TileFactory.SpinningMount(),
        TileFactory.SpinningMount(),
        TileFactory.Caramba(),
        TileFactory.Jungle(),
        TileFactory.Jungle(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Hole(),
        TileFactory.Quake(),
        TileFactory.Quake(),
        // 16 пустых клеток
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(2),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(3),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4),
        TileFactory.Empty(4)
    ];
}