using JackalWebHost2.Models;

namespace JackalWebHost2.Data.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    Task<User?> GetUser(long id, CancellationToken token);

    Task<IList<User>> GetUsers(long[] ids, CancellationToken token);

    /// <summary>
    /// Получить пользователя по логину
    /// </summary>
    Task<User?> GetUser(string login, CancellationToken token);
    
    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    Task<User> CreateUser(string login, CancellationToken token);
}