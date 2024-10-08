﻿using JackalWebHost2.Controllers.Models;
using JackalWebHost2.Models;
using JackalWebHost2.Services;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Controllers;

[Obsolete]
[Route("/api/[controller]/[action]")]
public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Запуск игры
    /// </summary>
    [HttpPost]
    public async Task<JsonResult> MakeStart([FromBody] StartGameRequest request)
    {
        var result = await _gameService.StartGame(new StartGameModel
        {
            GameName = request.GameName,
            Settings = request.Settings
        });

        return Json(new
        {
            gameName = result.GameName,
            pirates = result.Pirates,
            map = result.Map,
            mapId = result.MapId,
            stat = result.Statistics,
            moves = result.Moves
        });
    }

    /// <summary>
    /// Ход игры
    /// </summary>
    [HttpPost]
    public async Task<JsonResult> MakeTurn([FromBody] TurnGameRequest request)
    {
        var result = await _gameService.MakeGameTurn(new TurnGameModel
        {
            GameName = request.GameName,
            TurnNum = request.TurnNum,
            PirateId = request.PirateId
        });

        return Json(new
        {
            pirateChanges = result.PirateChanges,
            changes = result.Changes,
            stat = result.Statistics,
            moves = result.Moves
        });
    }
}