using Jackal.Core.Players;

namespace JackalWebHost2.Controllers.Models.Leaderboard;

public class LeaderboardResponse
{
    public List<GamePlayerStat> Leaderboard { get; set; }
}