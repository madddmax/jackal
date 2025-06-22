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
    public async Task<LeaderboardResponse> GetLeaderboard(LeaderboardOrderByType? orderBy)
    {
        var rand = new Random();
        var orderByParam = orderBy ?? (LeaderboardOrderByType)rand.Next(0, 3);
        
        return new LeaderboardResponse
        {
            Leaderboard = await gamePlayerRepository.GetLeaderboard(orderByParam)
        };
    }
}