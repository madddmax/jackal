using JackalWebHost2.Models;
using JackalWebHost2.Models.Lobby;

namespace JackalWebHost2.Services;

public interface ILobbyService
{
    /// <summary>
    /// Создать лобби с заданными параметрами
    /// </summary>
    Task<Lobby> CreateLobby(User user, GameSettings gameSettings, CancellationToken token);
    
    /// <summary>
    /// Присоединиться к лобби
    /// </summary>
    Task<Lobby> JoinLobby(string lobbyId, User user, CancellationToken token);
    
    /// <summary>
    /// Начать игру из лобби
    /// </summary>
    Task<Lobby> StartGame(string lobbyId, User user, CancellationToken token);
    
    /// <summary>
    /// Выгнать игрока
    /// </summary>
    Task KickPlayer(string lobbyId, User kickInitiator, long kickTarget, CancellationToken token);
    
    /// <summary>
    /// Задать команду игроку
    /// </summary>
    Task AssignTeam(string lobbyId, User user, long assignFor, int teamId, CancellationToken token);
    
    /// <summary>
    /// Покинуть лобби
    /// </summary>
    Task LeaveLobby(long userId, CancellationToken token);
    
    /// <summary>
    /// Получить информацию о лобби
    /// </summary>
    Task<Lobby> GetLobbyInfo(string lobbyId, User user, CancellationToken token);
}