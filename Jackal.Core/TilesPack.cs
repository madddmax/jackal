using System.Collections.Generic;

namespace Jackal
{
    /// <summary>
    /// Набор карт
    /// </summary>
    public class TilesPack
    {
        private TilesPack()
        {
            Fill();
        }

        private void AddArrowDef(int arrowsCode, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var def = new TileParams() {Type=TileType.Arrow,ArrowsCode = arrowsCode};
                _list.Add(def);
            }
        }

        private void AddSpinningDef(int spirnningCount, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                var def = new TileParams(){Type=TileType.Spinning,SpinningCount = spirnningCount};
                _list.Add(def);
            }
        }

        private void AddDef(TileType tileType, int count = 1)
        {
            for (int i = 1; i <= count; i++)
            {
                var def = new TileParams() {Type = tileType};
                _list.Add(def);
            }
        }

        private void Fill()
        {
            const int total = MapGenerator.Size*MapGenerator.Size - 4;
            _list = new List<TileParams>(total);


            AddDef(TileType.Chest1, 5);
            AddDef(TileType.Chest2, 5);
            AddDef(TileType.Chest3, 3);
            AddDef(TileType.Chest4, 2);
            AddDef(TileType.Chest5, 1);
            AddDef(TileType.Fort, 2);
            AddDef(TileType.RespawnFort);
            AddDef(TileType.RumBarrel, 4);
            AddDef(TileType.Horse, 2);
            AddDef(TileType.Balloon, 2);
            AddDef(TileType.Airplane, 1);
            AddDef(TileType.Croc, 4);
            AddDef(TileType.Ice, 6);
			AddDef(TileType.Cannon, 2);

            //arrows
            for (int arrowType = 0; arrowType < ArrowsCodesHelper.ArrowsTypes.Length; arrowType++)
            {
                int arrowsCode = ArrowsCodesHelper.ArrowsTypes[arrowType];
                AddArrowDef(arrowsCode, 3);
            }
            AddSpinningDef(2, 5);
            AddSpinningDef(3, 4);
            AddSpinningDef(4, 2);
            AddSpinningDef(5, 1);

            AddDef(TileType.Trap, 3);

            AddDef(TileType.Canibal, 1);

            while (_list.Count < total)
            {
                AddDef(TileType.Grass);
            }
        }

        private List<TileParams> _list;

        public IReadOnlyList<TileParams> List
        {
            get
            {
                return _list;
            }
        }

        public static readonly TilesPack Instance = new TilesPack();
    }
}