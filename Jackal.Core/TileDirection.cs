using System;
using Newtonsoft.Json;

namespace Jackal
{
    public class TileDirection
    {
        [JsonProperty] 
        public readonly TilePosition From;
        [JsonProperty] 
        public readonly TilePosition To;

        public TileDirection(TilePosition from, TilePosition to)
        {
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            From = from;
            To = to;
        }

        public TileDirection()
        {
        }

        public bool Equals(TileDirection other)
        {
            return Equals(From, other.From) && Equals(To, other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TileDirection) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode()*397) ^ (To.GetHashCode());
            }
        }

        public static bool operator ==(TileDirection left, TileDirection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TileDirection left, TileDirection right)
        {
            return !Equals(left, right);
        }
    }
}