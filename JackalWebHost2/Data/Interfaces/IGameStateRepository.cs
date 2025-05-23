using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Models;

namespace JackalWebHost2.Data.Interfaces;

public interface IGameStateRepository
{
    /// <summary>
    /// Флаг наличия изменений в активных играх
    /// </summary>
    bool HasGamesChanges();

    /// <summary>
    /// Сброс флага наличия изменений в активных играх
    /// </summary>
    void ResetGamesChanges();

    /// <summary>
    /// Получить описание всех активных игр
    /// </summary>
    IList<GameCacheEntry> GetGamesEntries();

    /// <summary>
    /// Получить состояние игры
    /// </summary>
    Task<Game?> GetGame(long gameId);

    /// <summary>
    /// Создать новое игровое состояние
    /// </summary>
    Task CreateGame(User user, long gameId, Game game);

    /// <summary>
    /// Обновить состояние игры
    /// </summary>
    Task UpdateGame(long gameId, Game game);
}