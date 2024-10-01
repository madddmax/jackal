using JackalWebHost2.Models;
using Microsoft.AspNetCore.Mvc;

namespace JackalWebHost2.Services;

public interface IGameService
{
    /// <summary>
    /// Запуск игры
    /// </summary>
    Task<StartGameResult> StartGame([FromBody] StartGameModel request);

    /// <summary>
    /// Ход игры
    /// </summary>
    Task<TurnGameResult> MakeGameTurn([FromBody] TurnGameModel request);
}