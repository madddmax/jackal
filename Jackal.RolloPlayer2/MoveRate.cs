using System;
using System.Collections.Generic;
using System.Linq;
using Jackal.Core;

namespace Jackal.RolloPlayer2
{
	public class MoveRate
	{
		public Move Move;

		public override string ToString()
		{
			double max = 0;
			double applyToAll = 1;
			string maxName = "None";

			if (!RateItems.Any())
			{
				max = - double.MaxValue;
			}
			else
			{
				var list = RateItems.Where(r => !r.ApplyToAll).ToList();
				max = list.Any() ? list.Max(r => r.Rate) : 0;

				maxName = list.Any() ? list.First(r => Math.Abs(r.Rate - max) < 0.1).RateActionName : maxName;

				list = RateItems.Where(r => r.ApplyToAll).ToList();
				if (list.Any())
					list.ForEach(r => { applyToAll = applyToAll*r.Rate; });
			}

			return string.Format("{0} {1} applyToAll={2}, ", Rate, maxName, applyToAll);
		}

		public virtual double Rate
		{
			get
			{
				if (!RateItems.Any())
					return -double.MaxValue;

				var list = RateItems.Where(r => !r.ApplyToAll).ToList();
				var max = list.Any() ? list.Max(r => r.Rate) : 0;
				
				double applyToAll = 1;
				list = RateItems.Where(r => r.ApplyToAll).ToList();
				if (list.Any()) 
					list.ForEach(r => { applyToAll = applyToAll * r.Rate; });

				return max*applyToAll;
			}
		}

		public List<MoveRateItem> RateItems = new List<MoveRateItem>();

		public void AddRate(MoveRateItem item)
		{
			RateItems.Add(item);
		}

		public void AddRate(string rateActionName, double rate)
		{
			AddRate(new MoveRateItem(rateActionName, rate));
		}

		public void AddApplyToAllRate(string rateActionName, double rate)
		{
			AddRate(new MoveRateItem(rateActionName, rate){ApplyToAll = true});
		}
	}
}
