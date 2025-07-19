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
        // 108 значимых клеток
        TileParams.Coin(), // 1 монета - первый сундук берем всегда
        TileParams.Coin(), // 2
        TileParams.Coin(), // 3
        TileParams.Coin(), // 4
        TileParams.Coin(), // 5
        TileParams.Coin(2), // 7
        TileParams.Coin(2), // 9
        TileParams.Coin(2), // 11
        TileParams.Coin(2), // 13
        TileParams.Coin(3), // 16
        TileParams.Coin(3), // 19
        TileParams.Coin(3), // 21
        TileParams.Coin(4), // 25
        TileParams.Coin(4), // 29
        TileParams.Coin(5), // 34
        TileParams.BigCoin(), // 37
        TileParams.BigCoin(), // 40
        TileParams.Fort(),
        TileParams.Fort(),
        TileParams.RespawnFort(), // порядок RespawnFort и Cannibal важен для баланса
        TileParams.Cannibal(), // берем воскрешающий форт вместе с людоедом
        TileParams.RespawnFort(),
        TileParams.Cannibal(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        TileParams.RumBottle(2),
        TileParams.RumBottle(2),
        TileParams.RumBottle(3),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Balloon(),
        TileParams.Balloon(),
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
        TileParams.Lighthouse(),
        TileParams.Lighthouse(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
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
        TileParams.SpinningSwamp(),
        TileParams.SpinningMount(),
        TileParams.SpinningMount(),
        TileParams.Caramba(),
        TileParams.Jungle(),
        TileParams.Jungle(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Hole(),
        TileParams.Quake(),
        TileParams.Quake(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        // 12 пустых клеток
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(2),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(3),
        TileParams.Empty(4),
        TileParams.Empty(4),
        TileParams.Empty(4)
    ];
}