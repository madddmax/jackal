using System.Collections.Generic;

namespace Jackal.Core
{
    public class GetAllAvaliableMovesTask
    {
        public int TeamId;
        public List<CheckedPosition> alreadyCheckedList = new List<CheckedPosition>();
        public TilePosition FirstSource;
        public TilePosition PreviosSource;
        public bool NoCoinMoving ;
        public bool NoTrap;
        public bool NoCanibal;
        public bool NoRespawn;
        public bool NoJumpToWater;
        public bool NoFort;
    }
}