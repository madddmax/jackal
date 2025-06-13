using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepository(JackalDbContext jackalDbContext) : IGamePlayerRepository
{
    public async Task<List<GamePlayerStatModel>> GetLeaderboard()
    {
        var bestWinningPlayers = await jackalDbContext.GamePlayers
            .Where(p => p.Winner)
            .GroupBy(p => p.PlayerName)
            .Select(g => new
            {
                PlayerName = g.Key,
                TotalWin = g.Count(),
                TotalWinCoins = g.Sum(x => x.Coins)
            })
            .OrderByDescending(g => g.TotalWin)
            .ToListAsync();

        var number = 1;
        return bestWinningPlayers.Select(g => new GamePlayerStatModel
            {
                Number = number++,
                PlayerName = g.PlayerName,
                TotalWin = g.TotalWin,
                TotalWinCoins = g.TotalWinCoins
            })
            .ToList();
    }
}