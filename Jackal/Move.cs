﻿namespace Jackal
{
	public struct Move
	{
		public Pirate Pirate;

		public Position To;
		public bool WithCoins;

		public Move(Pirate pirate, Position to, bool withCoin)
		{
			Pirate = pirate;
			To = to;
			WithCoins = withCoin;
		}

		public override int GetHashCode()
		{
			return this.Pirate.GetHashCode() + this.To.GetHashCode() + this.WithCoins.GetHashCode();
		}

		public bool Equals(Move other)
		{
			return this.Pirate.Position == other.Pirate.Position
				   && this.To == other.To
				   && this.WithCoins == other.WithCoins;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is Move && Equals((Move)obj);
		}

		public static bool operator ==(Move left, Move right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Move left, Move right)
		{
			return !left.Equals(right);
		}

		public override string ToString()
		{
			return string.Format("{0},{1},{2}", Pirate.Position, To, WithCoins ? "+": "o");
		}
	}
}