using Jackal.Core;
using Jackal.Core.Players;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GamePlayerRepository(JackalDbContext jackalDbContext) : IGamePlayerRepository
{
    private static readonly TimeZoneInfo MskTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
    
    public async Task<List<GamePlayerStat>> GetBotLeaderboard()
    {
        var groupedBotPlayers = jackalDbContext.GamePlayers
            .Where(p => p.UserId == null && p.Game.GameOver)
            .GroupBy(p => p.PlayerName);

        var botStat = await SelectStat(groupedBotPlayers);
        
        return botStat
            .OrderByDescending(g => g.TotalWin)
            .ToList();
    }

    public async Task<List<GamePlayerStat>> GetHumanLeaderboard()
    {
        var groupedHumanPlayers = jackalDbContext.GamePlayers
            .Where(p => p.UserId != null && p.Game.GameOver)
            .GroupBy(p => p.PlayerName);

        var humanStat = await SelectStat(groupedHumanPlayers);
        
        return humanStat
            .OrderByDescending(g => g.TotalWin)
            .ToList();
    }
    
    public async Task<List<GamePlayerStat>> GetTwoHumanInTeamLeaderboard()
    {
        var onlyHumanGameIds = jackalDbContext.GamePlayers
            .Where(g => g.Game.GameOver && g.Game.GameMode == GameModeType.TwoPlayersInTeam)
            .GroupBy(g => g.GameId)
            .Where(g => g
                .Where(p => p.UserId != null)
                .Select(p => p.UserId)
                .Distinct()
                .Count() == 4
            )
            .Select(g => g.Key);
        
        var groupedTwoHumanInTeamPlayers = jackalDbContext.GamePlayers
            .Where(p => onlyHumanGameIds.Contains(p.GameId))
            .GroupBy(p => p.PlayerName);
        
        var twoHumanInTeamStat = await SelectStat(groupedTwoHumanInTeamPlayers);

        return twoHumanInTeamStat
            .OrderByDescending(g => g.TotalWin)
            .ToList();
    }

    private static async Task<List<GamePlayerStat>> SelectStat(IQueryable<IGrouping<string, GamePlayerEntity>> groupedPlayers)
    {
        var (todayStartUtc, weekStartUtc, monthStartUtc) = GetComparedDates();

        return await groupedPlayers
            .Select(g => new GamePlayerStat
            {
                PlayerId = g.First().Id,
                PlayerName = g.Key,
                WinCountToday = g.Count(x => x.Game.Created >= todayStartUtc && x.Winner),
                WinCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc && x.Winner),
                WinCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc && x.Winner),
                TotalWin = g.Count(x => x.Winner),
                LoseCountToday = g.Count(x => x.Game.Created >= todayStartUtc && !x.Winner),
                LoseCountThisWeek = g.Count(x => x.Game.Created >= weekStartUtc && !x.Winner),
                LoseCountThisMonth = g.Count(x => x.Game.Created >= monthStartUtc && !x.Winner),
                TotalLose = g.Count(x => !x.Winner),
                TotalCoins = g.Sum(x => x.Coins)
            })
            .ToListAsync();
    }

    private static (DateTime todayStartUtc, DateTime weekStartUtc, DateTime monthStartUtc) GetComparedDates()
    {
        var nowUtc = DateTime.UtcNow;
        var todayStartMsk = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, MskTimeZone).Date;
        
        var todayStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk, MskTimeZone);
        var weekStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-7), MskTimeZone);
        var monthStartUtc = TimeZoneInfo.ConvertTimeToUtc(todayStartMsk.AddDays(-30), MskTimeZone);
        
        return new ValueTuple<DateTime, DateTime, DateTime>(todayStartUtc, weekStartUtc, monthStartUtc);
    }
}