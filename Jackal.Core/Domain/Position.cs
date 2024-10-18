using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public record Position
{
    [JsonProperty] 
    public readonly int X;

    [JsonProperty] 
    public readonly int Y;

    public Position()
    {
    }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Position(Position position)
    {
        X = position.X;
        Y = position.Y;
    }

    public static Position GetDelta(Position from, Position to)
    {
        return new Position(to.X - from.X, to.Y - from.Y);
    }

    public static Position AddDelta(Position pos, Position delta)
    {
        return new Position(pos.X + delta.X, pos.Y + delta.Y);
    }
}