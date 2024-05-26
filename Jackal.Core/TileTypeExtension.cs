namespace Jackal
{
    public static class TileTypeExtension
    {
        public static bool IsGold(this TileType source)
        {
            return source == TileType.Chest1 || source == TileType.Chest2 || source == TileType.Chest3 || source == TileType.Chest4 || source == TileType.Chest5;
        }

        /// <summary>
        /// Клетка требует немедленного движения по попаданию на неё?
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool RequreImmediateMove(this TileType type)
        {
            return (type == TileType.Arrow
                    || type == TileType.Horse
					|| type == TileType.Cannon
                    || type == TileType.Balloon
                    || type == TileType.Ice
                    || type == TileType.Croc
                    || type == TileType.Airplane);
        }

        public static bool IsFort(this TileType source)
        {
            return source == TileType.Fort || source == TileType.RespawnFort;
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