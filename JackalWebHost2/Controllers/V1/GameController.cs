using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers.V1;

[AllowAnonymous]
[Route("/api/v1/game")]
public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Старт новой игры
    /// </summary>
    [HttpPost("start")]
    public async Task<StartGameResponse> Start([FromBody] StartGameRequest request)
    {
        var result = await _gameService.StartGame(new StartGameModel
        {
            GameName = request.GameName,
            Settings = request.Settings
        });

        return new StartGameResponse
        {
            GameName = result.GameName,
            Pirates = result.Pirates,
            Map = result.Map,
            MapId = result.MapId,
            Stats = result.Statistics,
            Moves = result.Moves
        };
    }

    /// <summary>
    /// Ход игры
    /// </summary>
    [HttpPost("move")]
    public async Task<TurnGameResponse> Move([FromBody] TurnGameRequest request)
    {
        var result = await _gameService.MakeGameTurn(new TurnGameModel
        {
            GameName = request.GameName,
            TurnNum = request.TurnNum,
            PirateId = request.PirateId
        });

        return new TurnGameResponse
        {
            PirateChanges = result.PirateChanges,
            Changes = result.Changes,
            Stats = result.Statistics,
            Moves = result.Moves
        };
    }
}