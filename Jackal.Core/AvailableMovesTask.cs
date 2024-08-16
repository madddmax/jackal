using System.Collections.Generic;

namespace Jackal.Core
{
    public class AvailableMovesTask(int teamId, TilePosition source, TilePosition prev)
    {
        public readonly List<CheckedPosition> AlreadyCheckedList = [];
        
        public readonly int TeamId = teamId;

        public readonly TilePosition Source = source;
        
        public readonly TilePosition Prev = prev;
    }
}