using Jackal.Core.Players;

namespace JackalWebHost2.Data.Interfaces;

public interface IGamePlayerRepository
{
    Task<List<GamePlayerStat>> GetLeaderboard();
    
    Task<List<GamePlayerStat>> GetTwoHumanInTeamLeaderboard();
}