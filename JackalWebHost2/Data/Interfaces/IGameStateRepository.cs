using JackalWebHost2.Models;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameStateRepository
{
    /// <summary>
    /// Получить состояние игры
    /// </summary>
    Task<GameState?> GetGameState(string gameName);

    /// <summary>
    /// Создать новое игровое состояние
    /// </summary>
    Task CreateGameState(string gameName, GameState gameState);

    /// <summary>
    /// Обновить состояние игры
    /// </summary>
    Task UpdateGameState(string gameName, GameState gameState);
}