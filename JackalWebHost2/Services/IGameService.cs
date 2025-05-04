using JackalWebHost2.Models;

namespace JackalWebHost2.Services;

public interface IGameService
{
    /// <summary>
    /// Загрузка игры
    /// </summary>
    Task<LoadGameResult> LoadGame(long userId, long gameId);
    
    /// <summary>
    /// Запуск игры
    /// </summary>
    Task<StartGameResult> StartGame(long userId, StartGameModel request);

    /// <summary>
    /// Ход игры
    /// </summary>
    Task<TurnGameResult> MakeGameTurn(long userId, TurnGameModel request);
}