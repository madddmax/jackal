using JackalWebHost2.Models;

namespace JackalWebHost2.Services;

public interface IFastUserService
{
    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    Task<User?> GetUser(long id, CancellationToken token);
    
    /// <summary>
    /// Создать нового пользователя
    /// </summary>
    Task<User> CreateUser(string login, CancellationToken token);
}