using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Models;

namespace JackalWebHost2.Data.Interfaces;

public interface IStateRepository<T> where T : class, ICompletable
{
    /// <summary>
    /// Флаг наличия изменений
    /// </summary>
    bool HasChanges();

    /// <summary>
    /// Сброс флага наличия изменений
    /// </summary>
    void ResetChanges();

    /// <summary>
    /// Получить описание всех активных сущностей
    /// </summary>
    IList<CacheEntry> GetEntries();

    /// <summary>
    /// Получить сущность
    /// </summary>
    T? GetObject(long objectId);

    /// <summary>
    /// Создать новую сущность
    /// </summary>
    void CreateObject(User user, long objectId, T value);
    void CreateObject(User user, long objectId, T value, HashSet<User> players);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    void UpdateObject(long objectId, T value);
    void UpdateObject(long objectId, T value, HashSet<User>? players);
}