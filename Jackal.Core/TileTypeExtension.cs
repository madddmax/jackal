namespace Jackal.Core
{
    public static class TileTypeExtension
    {
        /// <summary>
        /// Клетка требует немедленного движения по попаданию на неё?
        /// </summary>
        public static bool RequireImmediateMove(this TileType type, bool used = false)
        {
            return type == TileType.Arrow
                   || type == TileType.Horse
                   || type == TileType.Cannon
                   || type == TileType.Balloon
                   || type == TileType.Ice
                   || type == TileType.Crocodile
                   || (type == TileType.Airplane && !used);
        }

        public static int CoinsCount(this TileType source)
        {
            int coins;
            switch (source)
            {
                case TileType.Chest1:
                    coins = 1;
                    break;
                case TileType.Chest2:
                    coins = 2;
                    break;
                case TileType.Chest3:
                    coins = 3;
                    break;
                case TileType.Chest4:
                    coins = 4;
                    break;
                case TileType.Chest5:
                    coins = 5;
                    break;
                default:
                    coins = 0;
                    break;
            }
            return coins;
        }
    }
}