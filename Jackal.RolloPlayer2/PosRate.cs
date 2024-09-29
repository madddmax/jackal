using Jackal.Core;

namespace Jackal.RolloPlayer2;

public class PosRate
{
	public PosRate(Position pos)
	{
		Pos = pos;
	}

	public Position Pos;
	public double Rate = 1;
}