using Jackal.Core;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameStateRepository
{
    /// <summary>
    /// Получить состояние игры
    /// </summary>
    Task<Game?> GetGame(long gameId);

    /// <summary>
    /// Создать новое игровое состояние
    /// </summary>
    Task CreateGame(long gameId, Game game);

    /// <summary>
    /// Обновить состояние игры
    /// </summary>
    Task UpdateGame(long gameId, Game game);
}