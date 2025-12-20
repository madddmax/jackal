using Jackal.Core.Players;

namespace JackalWebHost2.Data.Interfaces;

public interface IGamePlayerRepository
{
    Task<List<GamePlayerStat>> GetBotLeaderboard();
    
    Task<List<GamePlayerStat>> GetHumanLeaderboard();
    
    Task<List<GamePlayerStat>> GetTwoHumanInTeamLeaderboard();
}