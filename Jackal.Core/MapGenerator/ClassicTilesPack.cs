using System;
using System.Collections.Generic;

namespace Jackal.Core
{
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
            // 90 значимых клеток
            new TileParams(TileType.Chest1), // 1 монета
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
            new TileParams(TileType.RespawnFort),
            new TileParams(TileType.RespawnFort),
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
            new TileParams(TileType.Cannibal),
            new TileParams(TileType.Cannibal),
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
            // 30 пустых клеток
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass),
            new TileParams(TileType.Grass)
        ];    

        /// <summary>
        /// Количество монет на карте
        /// </summary>
        public int CoinsOnMap { get; private set; }
        
        /// <summary>
        /// Клетки которые попали в игру
        /// </summary>
        public List<TileParams> List { get; }
        
        public ClassicTilesPack(Random rand, int mapSize)
        {
            CoinsOnMap = 0;
            
            var landSize = mapSize - 2;
            var totalTiles = landSize * landSize - 4;
            List = new List<TileParams>(totalTiles);
            
            // выбираем клетки которые будем играть
            for (var i = 0; i < totalTiles; i++)
            {
                // без рэндома выбираем обязательный сундук с 1 монетой
                var index = i != 0 
                    ? rand.Next(0, _wholeSetOfTiles.Length - i) 
                    : 0;
                
                List.Add(_wholeSetOfTiles[index]);
                UpdateCoinsOnMap(_wholeSetOfTiles[index].Type);

                _wholeSetOfTiles[index] = _wholeSetOfTiles[_wholeSetOfTiles.Length - 1 - i];
            }
        }

        private void UpdateCoinsOnMap(TileType tileType) =>
            CoinsOnMap += tileType switch
            {
                TileType.Chest1 => 1,
                TileType.Chest2 => 2,
                TileType.Chest3 => 3,
                TileType.Chest4 => 4,
                TileType.Chest5 => 5,
                _ => 0
            };
    }
}