using Jackal.Core.Actions;

namespace Jackal.Core
{
    public class AvailableMove
    {
        public TilePosition Source;
        public TilePosition Target;
        public MoveType MoveType=MoveType.Usual;
        public GameActionList ActionList;

        public AvailableMove()
        {
        }

        public AvailableMove(TilePosition source, TilePosition target, params IGameAction[] actions)
        {
            Source = source;
            Target = target;
            ActionList = new GameActionList(actions);
        }
    }
}