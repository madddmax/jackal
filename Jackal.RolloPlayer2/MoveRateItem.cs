namespace Jackal.RolloPlayer2;

/// <summary>
/// Вклад каждого оценщика в итоговую оценку
/// </summary>
public class MoveRateItem
{
	public MoveRateItem(string rateActionName, double rate)
	{
		RateActionName = rateActionName;
		Rate = rate;
	}

	public string RateActionName;
	public double Rate;
	public bool ApplyToAll;
}