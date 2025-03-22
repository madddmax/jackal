using Jackal.Core;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameStateRepository
{
    /// <summary>
    /// Получить состояние игры
    /// </summary>
    Task<Game?> GetGame(string gameName);

    /// <summary>
    /// Создать новое игровое состояние
    /// </summary>
    Task CreateGame(string gameName, Game game);

    /// <summary>
    /// Обновить состояние игры
    /// </summary>
    Task UpdateGame(string gameName, Game game);
}