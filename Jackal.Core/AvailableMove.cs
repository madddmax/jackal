using Jackal.Core.Actions;
using Jackal.Core.Domain;

namespace Jackal.Core;

public class AvailableMove
{
    public TilePosition Source;
    public TilePosition Target;
    public Position? Prev;
    public MoveType MoveType = MoveType.Usual;
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