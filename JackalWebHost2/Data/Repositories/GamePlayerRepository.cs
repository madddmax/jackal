using Jackal.Core.Players;
using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepository(JackalDbContext jackalDbContext) : IGamePlayerRepository
{
    public async Task<List<GamePlayerStat>> GetLeaderboard()
    {
        var bestWinningPlayers = await jackalDbContext.GamePlayers
            .Where(p => p.UserId != null)
            .GroupBy(p => p.PlayerName)
            .Select(g => new
            {
                PlayerName = g.Key,
                TotalWin = g.Count(x => x.Winner),
                TotalCoins = g.Sum(x => x.Coins),
                GamesCount = g.Count()
            })
            .Where(g => g.TotalWin > 0)
            .ToListAsync();
        
        return bestWinningPlayers.Select(g => new GamePlayerStat
            {
                PlayerName = g.PlayerName,
                TotalWin = g.TotalWin,
                TotalCoins = g.TotalCoins,
                GamesCount = g.GamesCount
            })
            .OrderByDescending(g => g.Rating)
            .ToList();
    }
}