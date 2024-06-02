using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jackal;
using Jackal.Players;
using JackalWebHost.Models;
using JackalWebHost.Service;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost.Controllers
{
    public class GameController : Controller
    {
        public static Dictionary<string, GameState> Session = new();

        private JsonSerializerOptions _options = new()
        {
            AllowTrailingCommas = false,
            DefaultBufferSize = 1024,
            DefaultIgnoreCondition = JsonIgnoreCondition.Never,
            DictionaryKeyPolicy = null,
            Encoder = null,
            IgnoreReadOnlyFields = false,
            IgnoreReadOnlyProperties = false,
            IncludeFields = true,
            MaxDepth = 10,
            NumberHandling = JsonNumberHandling.Strict,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = null,
            ReadCommentHandling = JsonCommentHandling.Disallow,
            ReferenceHandler = null,
            TypeInfoResolver = null,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
            WriteIndented = false
        };

        /// <summary>
        /// Главное окно
        /// </summary>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Запуск игры
        /// </summary>
        public JsonResult Start(string settings)
        {
            GameState gameState = new GameState();

            var gameSettings = JsonHelper.DeserialiazeWithType<GameSettings>(settings);

            IPlayer[] gamePlayers = new IPlayer[4];
            int index = 0;

            foreach (var pl in gameSettings.players)
            {
                switch (pl)
                {
                    case "robot":
                        gamePlayers[index++] = new SmartPlayer();
                        break;
                    case "human":
                        gamePlayers[index++] = new WebHumanPlayer();
                        break;
                    default:
                        gamePlayers[index++] = new SmartPlayer2();
                        break;
                }
            }

            while (index < 4)
            {
                gamePlayers[index++] = new SmartPlayer();
            }

            if (!gameSettings.mapId.HasValue)
                gameSettings.mapId = new Random().Next();

            gameState.board = new Board(gamePlayers, gameSettings.mapId.Value);
            gameState.game = new Game(gamePlayers, gameState.board);

            Session["test"] = gameState;
            //HttpContext.Session.Set("test", gameState);

            var service = new DrawService();
            var map = service.Map(gameState.board);

            return Json(new {
                gamename = "test",
                map = map,
                mapId = gameSettings.mapId.Value,
                stat = service.GetStatistics(gameState.game)
            }, _options);
        }

        /// <summary>
        /// Ход игры
        /// </summary>
        public JsonResult Turn(int? turnNum)
        {
            //var gameState = HttpContext.Session.Get<GameState>("test");
            var gameState = Session["test"];
            if (gameState == null)
                return Json(new { error = true });

            string prevBoardStr = JsonHelper.SerialiazeWithType(gameState.board);

            if (gameState.game.CurrentPlayer is WebHumanPlayer)
            {
                if (turnNum.HasValue)
                {
                    gameState.game.CurrentPlayer.SetHumanMove(turnNum.Value);
                    gameState.game.Turn();
                }
                else
                {
                    // если пользователь не сделал выбор, то перезапрашиваем ход
                }
            }
            else gameState.game.Turn();

            var prevBoard = JsonHelper.DeserialiazeWithType<Board>(prevBoardStr);

            var service = new DrawService();
            return Json(new {
                changes = service.Draw(gameState.board, prevBoard),
                stat = service.GetStatistics(gameState.game),
                moves = service.GetAvailableMoves(gameState.game)
            }, _options);
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
