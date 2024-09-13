using System.Collections.Generic;

namespace Jackal.Core
{
    /// <summary>
    /// Набор карт
    /// </summary>
    public class ClassicTilesPack
    {
        private readonly int _totalTiles;
        private readonly List<TileParams> _list;

        public IReadOnlyList<TileParams> List => _list;

        public int CoinsOnMap { get; private set; }
        
        public ClassicTilesPack(int mapSize)
        {
            int landSize = mapSize - 2;
            _totalTiles = landSize * landSize - 4;
            
            CoinsOnMap = 0;
            _list = new List<TileParams>(_totalTiles);
            
            AddDef(TileType.Chest1, 5);
            AddDef(TileType.Chest2, 5);
            AddDef(TileType.Chest3, 3);
            AddDef(TileType.Chest4, 2);
            AddDef(TileType.Chest5, 1);
            AddDef(TileType.Fort, 2);
            AddDef(TileType.RespawnFort, 2);
            AddDef(TileType.RumBarrel, 4);
            AddDef(TileType.Horse, 2);
            AddDef(TileType.Balloon, 2);
            AddDef(TileType.Airplane, 2);
            AddDef(TileType.Crocodile, 4);
            AddDef(TileType.Ice, 6);
            AddDef(TileType.Cannon, 2);
            
            foreach (var arrowsCode in ArrowsCodesHelper.ArrowsTypes)
            {
                AddArrowDef(arrowsCode, 3);
            }
            
            AddDef(TileType.Trap, 3);
            AddDef(TileType.Cannibal, 2);
            AddDef(TileType.Lighthouse, 2);
            AddDef(TileType.BenGunn, 2);
            
            AddSpinningDef(2, 5);
            AddSpinningDef(3, 4);
            AddSpinningDef(4, 2);
            AddSpinningDef(5, 1);
            
            while (_list.Count < _totalTiles)
            {
                AddDef(TileType.Grass);
            }
        }

        private void AddDef(TileType tileType, int count = 1)
        {
            for (int i = 1; i <= count; i++)
            {
                if (_list.Count == _totalTiles)
                    return;

                UpdateCoinsOnMap(tileType);

                var def = new TileParams { Type = tileType };
                _list.Add(def);
            }
        }
        
        private void AddArrowDef(int arrowsCode, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                if (_list.Count == _totalTiles)
                    return;

                var def = new TileParams { Type = TileType.Arrow, ArrowsCode = arrowsCode };
                _list.Add(def);
            }
        }

        private void AddSpinningDef(int spinningCount, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                if (_list.Count == _totalTiles)
                    return;

                var def = new TileParams { Type = TileType.Spinning, SpinningCount = spinningCount };
                _list.Add(def);
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