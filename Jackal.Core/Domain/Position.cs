using Newtonsoft.Json;

namespace Jackal.Core.Domain;

public class Position
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

    public Position(Position position):this(position.X,position.Y)
    {
    }

    public bool Equals(Position other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Position)obj);
    }

    public static bool operator ==(Position left, Position right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (X*397) ^ Y;
        }
    }

    public static Position GetDelta(Position from, Position to)
    {
        return new Position(to.X - from.X, to.Y - from.Y);
    }

    public static Position AddDelta(Position pos, Position delta)
    {
        return new Position(pos.X + delta.X, pos.Y + delta.Y);
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", X, Y);
    }
}