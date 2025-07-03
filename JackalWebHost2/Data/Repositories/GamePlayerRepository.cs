using Jackal.Core.Players;
using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepository(JackalDbContext jackalDbContext) : IGamePlayerRepository
{
    public async Task<List<GamePlayerStat>> GetLeaderboard(LeaderboardOrderByType orderBy)
    {
        var bestWinningPlayers = await jackalDbContext.GamePlayers
            .Where(p => p.UserId != null)
            .GroupBy(p => p.PlayerName)
            .Select(g => new GamePlayerStat
            {
                PlayerName = g.Key,
                TotalWin = g.Count(x => x.Winner),
                TotalCoins = g.Sum(x => x.Coins),
                GamesCount = g.Count()
            })
            .Where(g => g.TotalWin > 0)
            .ToListAsync();

        return orderBy switch
        {
            LeaderboardOrderByType.Rating => bestWinningPlayers.OrderByDescending(g => g.Rating).ToList(),
            LeaderboardOrderByType.TotalWin => bestWinningPlayers.OrderByDescending(g => g.TotalWin).ToList(),
            LeaderboardOrderByType.GamesCount => bestWinningPlayers.OrderByDescending(g => g.GamesCount).ToList(),
            LeaderboardOrderByType.TotalCoins => bestWinningPlayers.OrderByDescending(g => g.TotalCoins).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(orderBy), orderBy, null)
        };
    }
}