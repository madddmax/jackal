using System.Collections.Generic;

namespace Jackal.Core
{
    public class AvailableMovesTask
    {
        public int TeamId;
        public List<CheckedPosition> alreadyCheckedList = new List<CheckedPosition>();
        public TilePosition FirstSource;
        public TilePosition PreviosSource;
        public bool NoCoinMoving ;
        public bool NoTrap;
        public bool NoCanibal;
        public bool NoRespawn;
        public bool NoFort;
    }
}