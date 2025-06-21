using Jackal.Core.Players;
using JackalWebHost2.Controllers.Models.Leaderboard;

namespace JackalWebHost2.Data.Interfaces;

public interface IGamePlayerRepository
{
    Task<List<GamePlayerStat>> GetLeaderboard();
}