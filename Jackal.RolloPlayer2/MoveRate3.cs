using System;
using System.Linq;

namespace Jackal.RolloPlayer2;

public class MoveRate3: MoveRate
{
	public override string ToString()
	{
		double max = 0;
		double applyToAll = 1;
		string maxName = "None";

		if (!RateItems.Any())
		{
			max = 1;
		}
		else
		{
			var list = RateItems.Where(r => !r.ApplyToAll).ToList();
			max = list.Any() ? list.Max(r => r.Rate) : 1;

			maxName = list.Any() ? list.First(r => Math.Abs(r.Rate - max) < 0.1).RateActionName : maxName;

			list = RateItems.Where(r => r.ApplyToAll).ToList();
			if (list.Any())
				list.ForEach(r => { applyToAll = applyToAll * r.Rate; });
		}

		return string.Format("{0} {1} applyToAll={2}, ", Rate, maxName, applyToAll);
	}

	public override double Rate
	{
		get
		{
			if (!RateItems.Any())
				return 1;

			var list = RateItems.Where(r => !r.ApplyToAll).ToList();
			var max = list.Any() ? list.Max(r => r.Rate) : 1;

			double applyToAll = 1;
			list = RateItems.Where(r => r.ApplyToAll).ToList();
			if (list.Any())
				list.ForEach(r => { applyToAll = applyToAll * r.Rate; });

			return max * applyToAll;
		}
	}
}