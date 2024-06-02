using System;
using Newtonsoft.Json;

namespace Jackal.Core
{
    public class TilePosition
    {
        [JsonProperty] 
        public readonly Position Position;

        [JsonIgnore]
        public int X
        {
            get { return Position.X; }
        }

        [JsonIgnore]
        public int Y
        {
            get { return Position.Y; }
        }

        [JsonProperty] 
        public readonly int Level;

        public TilePosition(Position position, int level = 0)
        {
            if (position == null) throw new ArgumentNullException("position");
            if (level < 0 || level > 4) throw new ArgumentException("level");
            Position = position;
            Level = level;
        }

        protected bool Equals(TilePosition other)
        {
            return Position.Equals(other.Position) && Level == other.Level;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TilePosition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Position.GetHashCode()*397) ^ Level;
            }
        }

        public static bool operator ==(TilePosition left, TilePosition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TilePosition left, TilePosition right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("({0},{1},{2})", Position.X, Position.Y, Level);
        }
    }
}