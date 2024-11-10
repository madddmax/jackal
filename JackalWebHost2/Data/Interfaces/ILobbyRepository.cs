using JackalWebHost2.Models.Lobby;

namespace JackalWebHost2.Data.Interfaces;

public interface ILobbyRepository
{
    /// <summary>
    /// Создать лобби
    /// </summary>
    Task CreateLobby(Lobby lobby, CancellationToken token);
    
    /// <summary>
    /// Добавить пользователя в лобби
    /// </summary>
    Task AddUserToLobby(string lobbyId, LobbyMember lobbyMember, CancellationToken token);
    
    /// <summary>
    /// Получить лобби по пользователю
    /// </summary>
    Task<Lobby?> GetLobbyByUser(long userId, CancellationToken token);
    
    /// <summary>
    /// Получить данные по лобби
    /// </summary>
    Task<Lobby?> GetLobbyInfo(string lobbyId, bool includeDeleted, CancellationToken token);
    
    /// <summary>
    /// Удалить пользователя из всех лобби
    /// </summary>
    Task RemoveUserFromLobbies(long userId, CancellationToken token);
    
    /// <summary>
    /// Обновить метку времени lastSeen  пользователя лобби
    /// </summary>
    Task UpdateUserKeepAlive(string lobbyId, long userId, DateTimeOffset time, CancellationToken token);
    
    /// <summary>
    /// Закрыть лобби и проставить идентификатор игры (если есть)
    /// </summary>
    Task Close(string lobbyId, DateTimeOffset time, string? gameId, long[]? gameMembers, CancellationToken token);
    
    /// <summary>
    /// Выгнать всех пользователей из лобби
    /// </summary>
    Task RemoveUsersFromLobby(string lobbyId, CancellationToken token);

    /// <summary>
    /// Назначить пользователю команду в лобби
    /// </summary>
    Task AssignTeam(string lobbyId, long userId, long? teamId, CancellationToken token);
}