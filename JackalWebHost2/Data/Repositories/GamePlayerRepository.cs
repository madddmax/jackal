using Jackal.Core;
using Jackal.Core.Players;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepository(JackalDbContext jackalDbContext) : IGamePlayerRepository
{
    public async Task<List<GamePlayerStat>> GetLeaderboard()
    {
        TimeZoneInfo mskTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        
        DateTime nowUtc = DateTime.UtcNow;
        DateTime todayStartMsk = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, mskTimeZone).Date;
        
        DateTime todayStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk, mskTimeZone);
        DateTime weekStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-7), mskTimeZone);
        DateTime monthStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-30), mskTimeZone);
        
        var bestWinningPlayers = await jackalDbContext.GamePlayers
            .Where(g => g.Game.GameOver)
            .GroupBy(p => p.PlayerName)
            .Select(g => new GamePlayerStat
            {
                PlayerName = g.Key,
                WinCountToday = g.Count(x => x.Game.Created >= todayStartUtc && x.Winner),
                WinCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc && x.Winner),
                WinCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc && x.Winner),
                TotalWin = g.Count(x => x.Winner),
                GamesCountToday = g.Count(x => x.Game.Created >= todayStartUtc),
                GamesCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc),
                GamesCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc),
                GamesCountTotal = g.Count(),
                TotalCoins = g.Sum(x => x.Coins)
            })
            .ToListAsync();

        return bestWinningPlayers
            .OrderByDescending(g => g.TotalWin)
            .ToList();
    }
    
    public async Task<List<GamePlayerStat>> GetTwoHumanInTeamLeaderboard()
    {
        TimeZoneInfo mskTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        
        DateTime nowUtc = DateTime.UtcNow;
        DateTime todayStartMsk = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, mskTimeZone).Date;
        
        DateTime todayStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk, mskTimeZone);
        DateTime weekStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-7), mskTimeZone);
        DateTime monthStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-30), mskTimeZone);

        var onlyHumanGameIds = jackalDbContext.GamePlayers
            .Where(g => g.Game.GameOver && g.Game.GameMode == GameModeType.TwoPlayersInTeam)
            .GroupBy(g => g.GameId)
            .Where(g => g.Select(p => p.UserId).Distinct().Count() == 4)
            .Select(g => g.Key);
        
        var bestWinningPlayers = await jackalDbContext.GamePlayers
            .Where(p => onlyHumanGameIds.Contains(p.GameId))
            .GroupBy(p => p.PlayerName)
            .Select(g => new GamePlayerStat
            {
                PlayerName = g.Key,
                WinCountToday = g.Count(x => x.Game.Created >= todayStartUtc && x.Winner),
                WinCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc && x.Winner),
                WinCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc && x.Winner),
                TotalWin = g.Count(x => x.Winner),
                GamesCountToday = g.Count(x => x.Game.Created >= todayStartUtc),
                GamesCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc),
                GamesCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc),
                GamesCountTotal = g.Count(),
                TotalCoins = g.Sum(x => x.Coins)
            })
            .ToListAsync();

        return bestWinningPlayers
            .OrderByDescending(g => g.TotalWin)
            .ToList();
    }
}