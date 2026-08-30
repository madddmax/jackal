using Jackal.Core.Domain;

namespace Jackal.Core.MapGenerator.TilesPack;

/// <summary>
/// Несбалансированный игровой набор от madddmax-а,
/// все клетки могут выпасть с одинаковой вероятностью,
/// максимум 5 штук одного вида
/// </summary>
public class ImbalanceTilesPack : ITilesPack
{
    /// <summary>
    /// 180 клеток
    /// </summary>
    public TileParams[] AllTiles { get; } =
    [
        TileParams.Coin(), // 1 монета - первый сундук берем всегда
        TileParams.Coin(),
        TileParams.Coin(),
        TileParams.Coin(),
        TileParams.Coin(),
        
        TileParams.Coin(2),
        TileParams.Coin(2),
        TileParams.Coin(2),
        TileParams.Coin(2),
        TileParams.Coin(2),
        
        // 10
        TileParams.Coin(3),
        TileParams.Coin(3),
        TileParams.Coin(3),
        TileParams.Coin(3),
        TileParams.Coin(3),
        
        TileParams.Coin(4),
        TileParams.Coin(4),
        TileParams.Coin(4),
        TileParams.Coin(5),
        TileParams.Coin(5),
        
        // 20
        TileParams.BigCoin(),
        TileParams.BigCoin(),
        TileParams.BigCoin(),
        TileParams.BigCoin(),
        TileParams.BigCoin(),
        
        TileParams.Fort(),
        TileParams.Fort(),
        TileParams.Fort(),
        TileParams.Fort(),
        TileParams.Fort(),
        
        // 30
        TileParams.RespawnFort(),
        TileParams.RespawnFort(),
        TileParams.RespawnFort(),
        TileParams.RespawnFort(),
        TileParams.RespawnFort(),
        
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.Cannibal(),
        TileParams.Cannibal(),

        // 40
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        TileParams.RumBarrel(),
        
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        TileParams.RumBottle(),
        
        // 50
        TileParams.RumBottle(2),
        TileParams.RumBottle(2),
        TileParams.RumBottle(2),
        TileParams.RumBottle(3),
        TileParams.RumBottle(3),
        
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        TileParams.Horse(),
        
        // 60
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
        
        // 70
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        TileParams.Crocodile(),
        
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),
        TileParams.Ice(),

        // 80
        TileParams.Cannon(),
        TileParams.Cannon(),
        TileParams.Cannon(),
        TileParams.Cannon(),
        TileParams.Cannon(),
        
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        TileParams.OneArrowUp(),
        
        // 90
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        TileParams.OneArrowDiagonal(),
        
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        TileParams.TwoArrowsDiagonal(),
        
        // 100
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        TileParams.TwoArrowsLeftRight(),
        
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        TileParams.ThreeArrows(),
        
        // 110
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        TileParams.FourArrowsPerpendicular(),
        
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        TileParams.FourArrowsDiagonal(),
        
        // 120
        TileParams.Trap(),
        TileParams.Trap(),
        TileParams.Trap(),
        TileParams.Trap(),
        TileParams.Trap(),
        
        TileParams.Lighthouse(),
        TileParams.Lighthouse(),
        TileParams.Lighthouse(),
        TileParams.Lighthouse(),
        TileParams.Lighthouse(),
        
        // 130
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        TileParams.BenGunn(),
        
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        TileParams.SpinningForest(),
        
        // 140
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        TileParams.SpinningDesert(),
        
        TileParams.SpinningSwamp(),
        TileParams.SpinningSwamp(),
        TileParams.SpinningSwamp(),
        TileParams.SpinningMount(),
        TileParams.SpinningMount(),
        
        // 150
        TileParams.Caramba(),
        TileParams.Caramba(),
        TileParams.Caramba(),
        TileParams.Caramba(),
        TileParams.Caramba(),
        
        TileParams.Jungle(),
        TileParams.Jungle(),
        TileParams.Jungle(),
        TileParams.Jungle(),
        TileParams.Jungle(),
        
        // 160
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
        
        // 170
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),
        TileParams.Cannabis(),

        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty(),
        TileParams.Empty()
        
        // 180
    ];
}