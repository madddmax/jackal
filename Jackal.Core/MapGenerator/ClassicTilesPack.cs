using System;
using System.Collections.Generic;

namespace Jackal.Core;

/// <summary>
/// Классический игровой набор Финам-Шакал
/// </summary>
public class ClassicTilesPack
{
    /// <summary>
    /// Общий набор из 120 клеток,
    /// самая большая карта 13x13 имеет 117 клеток
    /// </summary>
    private readonly TileParams[] _wholeSetOfTiles =
    [
        // 94 значимых клетки
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
        new TileParams(TileType.Horse),
        new TileParams(TileType.Balloon),
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
        new TileParams(TileType.Caramba),
        new TileParams(TileType.Jungle),
        new TileParams(TileType.Jungle),
        // 26 пустых клеток
        new TileParams(TileType.Grass, 0),
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

    /// <summary>
    /// Количество монет на карте
    /// </summary>
    public int CoinsOnMap { get; private set; }
        
    /// <summary>
    /// Клетки которые будем играть
    /// </summary>
    public List<TileParams> List { get; }
        
    public ClassicTilesPack(Random rand, int mapSize)
    {
        CoinsOnMap = 0;
            
        var landSize = mapSize - 2;
        var totalTiles = landSize * landSize - 4;
        List = new List<TileParams>(totalTiles);

        // выбираем обязательный сундук с 1 монетой
        bool random = false;
        int selectedIndex = 0;
            
        for (var i = 0; i < totalTiles; i++)
        {
            var index = random 
                ? rand.Next(0, _wholeSetOfTiles.Length - i) 
                : selectedIndex;
                
            List.Add(_wholeSetOfTiles[index]);
            CoinsOnMap += _wholeSetOfTiles[index].Type.CoinsCount();

            switch (_wholeSetOfTiles[index].Type)
            {
                case TileType.Cannibal:
                    // выбираем воскрешающий форт к людоеду
                    random = false;
                    selectedIndex = index - 1;
                    break;
                case TileType.RespawnFort:
                    // выбираем людоеда к воскрешающему форту
                    random = false;
                    selectedIndex = index + 1;
                    break;
                default:
                    random = true;
                    break;
            }

            // сдвигаем оставшиеся клетки в наборе, последнюю ставим на место выбранной
            _wholeSetOfTiles[index] = _wholeSetOfTiles[_wholeSetOfTiles.Length - 1 - i];
        }
    }
}