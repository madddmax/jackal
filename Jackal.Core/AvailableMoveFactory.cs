using Jackal.Core.Actions;
using Jackal.Core.Domain;

namespace Jackal.Core;

public static class AvailableMoveFactory
{
    public static AvailableMove RespawnMove(TilePosition from, TilePosition to)
    {
        var respawnAction = new RespawnAction();
        return new AvailableMove(from, to, respawnAction)
        {
            MoveType = MoveType.WithRespawn
        };
    }

    public static AvailableMove QuakeMove(TilePosition from, TilePosition to, TilePosition prev)
    {
        var quakeAction = new QuakeAction(prev, to);
        return new AvailableMove(from, to, quakeAction)
        {
            MoveType = MoveType.WithQuake
        };
    }

    public static AvailableMove UsualMove(TilePosition from, TilePosition to, TilePosition prev)
    {
        var moving = new MovingAction(from, to, prev);
        return new AvailableMove(from, to, moving);
    }

    public static AvailableMove CoinMove(TilePosition from, TilePosition to, TilePosition prev)
    {
        var movingWithCoin = new MovingWithCoinAction(from, to, prev);
        return new AvailableMove(from, to, movingWithCoin)
        {
            MoveType = MoveType.WithCoin
        };
    }
}