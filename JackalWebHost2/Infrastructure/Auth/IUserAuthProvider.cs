using JackalWebHost2.Models;

namespace JackalWebHost2.Infrastructure.Auth;

public interface IUserAuthProvider
{
    /// <summary>
    /// Получить залогиненного пользователя из запроса
    /// </summary>
    bool TryGetUser(out User? user);
    
    /// <summary>
    /// Получить залогиненного пользователя из запроса или выкинуть исключение
    /// </summary>
    User GetUser();
    
    /// <summary>
    /// Установить залогиненного пользователя для запроса
    /// </summary>
    void SetUser(User user);
}