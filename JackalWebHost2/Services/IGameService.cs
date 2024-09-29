using JackalWebHost2.Models;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Services;

public interface IGameService
{
    /// <summary>
    /// Запуск игры
    /// </summary>
    StartGameResult StartGame([FromBody] StartGameModel request);

    /// <summary>
    /// Ход игры
    /// </summary>
    TurnGameResult MakeGameTurn([FromBody] TurnGameModel request);
}