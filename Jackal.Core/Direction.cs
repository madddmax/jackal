using System;
using System.ComponentModel.Design.Serialization;
using Newtonsoft.Json;

namespace Jackal
{
    public class Direction
    {
        [JsonProperty]
        public readonly TilePosition From;
        [JsonProperty]
        public readonly TilePosition To;

        public Direction(TilePosition from, TilePosition to)
        {
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            From = from;
            To = to;
        }

        protected bool Equals(Direction other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Direction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode()*397) ^ (To.GetHashCode());
            }
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !Equals(left, right);
        }

        public Position GetDelta()
        {
            return Position.GetDelta(From.Position, To.Position);
        }
    }
}