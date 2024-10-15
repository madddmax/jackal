using Jackal.Core.Domain;

namespace Jackal.Core;

public class CheckedPosition
{
    public TilePosition Position;
    public Position? IncomeDelta;

    public CheckedPosition(TilePosition position, Position? incomeDelta = null)
    {
        Position = position;
        IncomeDelta = incomeDelta;
    }
}