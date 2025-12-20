using JackalWebHost2.Controllers.Models.Leaderboard;
using JackalWebHost2.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[AllowAnonymous]
[Route("/api/v1/leaderboard")]
public class LeaderboardController(IGamePlayerRepository gamePlayerRepository) : Controller
{
    /// <summary>
    /// Таблица лидеров среди игроков общая
    /// </summary>
    [HttpGet]
    public async Task<LeaderboardResponse> GetHumanLeaderboard() =>
        new()
        {
            Leaderboard = await gamePlayerRepository.GetHumanLeaderboard()
        };
    
    /// <summary>
    /// Таблица лидеров среди ботов общая
    /// </summary>
    [HttpGet("bot-all")]
    public async Task<LeaderboardResponse> GetBotLeaderboard() =>
        new()
        {
            Leaderboard = await gamePlayerRepository.GetBotLeaderboard()
        };
    
    /// <summary>
    /// Таблица лидеров среди игроков турнира 2x2
    /// </summary>
    [HttpGet("two-human-in-team")]
    public async Task<LeaderboardResponse> GetTwoHumanInTeamLeaderboard() =>
        new()
        {
            Leaderboard = await gamePlayerRepository.GetTwoHumanInTeamLeaderboard()
        };
}