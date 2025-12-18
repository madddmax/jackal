using Jackal.Core.Players;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepositoryStub : IGamePlayerRepository
{
    public Task<List<GamePlayerStat>> GetLeaderboard()
    {
        return Task.FromResult(new List<GamePlayerStat>());
    }

    public Task<List<GamePlayerStat>> GetTwoHumanInTeamLeaderboard()
    {
        return Task.FromResult(new List<GamePlayerStat>());
    }
}