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
    /// Таблица лидеров
    /// </summary>
    [HttpGet]
    public async Task<LeaderboardResponse> GetLeaderboard(LeaderboardOrderByType? orderBy) =>
        new()
        {
            Leaderboard = await gamePlayerRepository.GetLeaderboard(orderBy ?? LeaderboardOrderByType.TotalCoins)
        };
}