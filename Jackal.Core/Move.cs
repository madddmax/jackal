using System;
using Newtonsoft.Json;

namespace Jackal
{
    public class Move : TileDirection
	{
        [JsonProperty]
        public readonly MoveType Type;

        public bool WithCoins { get { return Type == MoveType.WithCoin; } }
	    public bool WithRespawn{ get { return Type == MoveType.WithRespawn; } }

        public bool Equals(Move other)
        {
            return base.Equals(other) && Type.Equals(other.Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Move) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Move left, Move right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !Equals(left, right);
        }

        public Move():base()
        {
        }

        public Move(TilePosition from, TilePosition to, MoveType type = MoveType.Usual)
            : base(from, to)
        {
            Type = type;
        }

        public Move(Position from, Position to, MoveType type = MoveType.Usual)
            : this(new TilePosition(from), new TilePosition(to), type)
        {
        }
	}
}