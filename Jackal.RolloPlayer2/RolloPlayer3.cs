using System.Linq;

namespace Jackal.RolloPlayer2
{
	public class RolloPlayer3: RolloPlayer2
	{
		protected override Rater CreateRater(Board board, int teamId, Settings settings)
		{
			return new Rater3(board, teamId, settings);
		}

		public override int OnMove(GameState gameState)
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
					return i;
			}
			return 0;
		}
	}
}
