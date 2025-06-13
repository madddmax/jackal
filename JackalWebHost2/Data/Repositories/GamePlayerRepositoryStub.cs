using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepositoryStub : IGamePlayerRepository
{
    public Task<List<GamePlayerStatModel>> GetLeaderboard()
    {
        return Task.FromResult(new List<GamePlayerStatModel>());
    }
}