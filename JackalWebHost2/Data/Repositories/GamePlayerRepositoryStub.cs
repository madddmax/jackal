using Jackal.Core.Players;
using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepositoryStub : IGamePlayerRepository
{
    public Task<List<GamePlayerStat>> GetLeaderboard(LeaderboardOrderByType orderBy)
    {
        return Task.FromResult(new List<GamePlayerStat>());
    }
}