using System;
using System.Linq;
using Jackal.Core;

namespace Jackal.RolloPlayer2;

public class RolloPlayer2: RolloPlayer
{
	protected override Rater CreateRater(Board board, int teamId, Settings settings)
	{
		return new Rater2(board, teamId, settings);
	}

	public override (int moveNum, Guid? pirateId) OnMove(GameState gameState)
	{
		var board = gameState.Board;
		var availableMoves = gameState.AvailableMoves;
		var teamId = gameState.TeamId;


		var rater = CreateRater(board, teamId, Settings.Default);

		var moveRates = availableMoves.Select(rater.Rate).ToList();

		var maxRate = moveRates.Max(mr => mr.Rate);

		var result = moveRates.Where(mr => mr.Rate >= maxRate).OrderByDescending(mr => mr.RateItems.Sum(i => i.Rate)).First().Move;

		for (var i = 0; i < availableMoves.Length; i++)
		{
			if (availableMoves[i] == result)
				return (i, null);
		}
		return (0, null);
	}
}