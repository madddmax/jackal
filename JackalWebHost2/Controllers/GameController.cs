﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Jackal.Core;
using Jackal.Core.Players;
using JackalWebHost.Models;
using JackalWebHost.Service;
using JackalWebHost2.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost.Controllers
{
    public class GameController : Controller
    {
        private readonly IMemoryCache _gamesSessionsCache;

        private readonly MemoryCacheEntryOptions _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1));
        
        public GameController(IMemoryCache gamesSessionsCache)
        {
            _gamesSessionsCache = gamesSessionsCache;
        }
        
        /// <summary>
        /// Главное окно
        /// </summary>
        public ActionResult Index()
        {
            var html = System.IO.File.ReadAllText(@"./wwwroot/dist/index.html");
            return base.Content(html, "text/html");
        }

        /// <summary>
        /// Запуск игры
        /// </summary>
        public JsonResult MakeStart([FromBody] StartGameModel request)
        {
            return InnerStart(request.GameName, request.Settings);
        }

        /// <summary>
        /// Запуск игры
        /// </summary>
        private JsonResult InnerStart(string gameName, GameSettings gameSettings)
        {
            GameState gameState = new GameState();

            IPlayer[] gamePlayers = new IPlayer[gameSettings.Players.Length];
            int index = 0;

            foreach (var player in gameSettings.Players)
            {
                gamePlayers[index++] = player switch
                {
                    "robot" => new SmartPlayer(),
                    "human" => new WebHumanPlayer(),
                    _ => new SmartPlayer2()
                };
            }

            if (!gameSettings.MapId.HasValue)
                gameSettings.MapId = new Random().Next();

            int mapSize = gameSettings.MapSize ?? 5;
            int piratesPerPlayer = 3;
            gameState.board = new Board(gamePlayers, gameSettings.MapId.Value, mapSize, piratesPerPlayer);
            gameState.game = new Game(gamePlayers, gameState.board);

            _gamesSessionsCache.Set(gameName, gameState, _cacheEntryOptions);

            var service = new DrawService();
            var map = service.Map(gameState.board);

            return Json(new {
                gameName,
                pirates = gameState.game.Board.AllPirates,
                map,
                mapId = gameSettings.MapId.Value,
                stat = DrawService.GetStatistics(gameState.game),
                moves = DrawService.GetAvailableMoves(gameState.game)
            });
        }
        
        /// <summary>
        /// Ход игры
        /// </summary>
        public JsonResult MakeTurn([FromBody] TurnGameModel request)
        {
            return Turn(request.GameName, request.TurnNum, request.PirateId);
        }

        /// <summary>
        /// Ход игры
        /// </summary>
        public JsonResult Turn(string gameName, int? turnNum, Guid? pirateId)
        {
            if (!_gamesSessionsCache.TryGetValue(gameName, out GameState? gameState) || 
                gameState == null)
            {
                return Json(new { error = true });
            }

            var prevBoardStr = JsonHelper.SerialiazeWithType(gameState.board);

            if (gameState.game.CurrentPlayer is WebHumanPlayer && turnNum.HasValue)
            {
                gameState.game.CurrentPlayer.SetHumanMove(turnNum.Value, pirateId);
            }

            gameState.game.Turn();

            var prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevBoardStr);

            var service = new DrawService();
            (List<PirateChange> pirateChanges, List<TileChange> tileChanges) = service.Draw(gameState.board, prevBoard);
            return Json(new {
                pirates = gameState.game.Board.AllPirates,
                pirateChanges,
                changes = tileChanges,
                stat = DrawService.GetStatistics(gameState.game),
                moves = DrawService.GetAvailableMoves(gameState.game)
            });
        }

        /// <summary>
        /// Сброс игры
        /// </summary>
        public JsonResult Reset()
        {
            HttpContext.Session.Set("data", Array.Empty<byte>());

            return Json(new { result = "ok" });
        }
    }
}
