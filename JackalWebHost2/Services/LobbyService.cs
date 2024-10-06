using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Models;
using JackalWebHost2.Models.Lobby;

namespace JackalWebHost2.Services;

public class LobbyService
{
    private readonly ILogger<LobbyService> _logger;
    private readonly ILobbyRepository _lobbyRepository;
    private readonly IGameService _gameService;
    private readonly TimeProvider _timeProvider;

    public LobbyService(
        ILogger<LobbyService> logger, 
        ILobbyRepository lobbyRepository, 
        IGameService gameService, 
        TimeProvider timeProvider)
    {
        _logger = logger;
        _lobbyRepository = lobbyRepository;
        _gameService = gameService;
        _timeProvider = timeProvider;
    }
    
    public async Task<Lobby> CreateLobby(User user, GameSettings gameSettings, CancellationToken token)
    {
        var userId = user.Id;
        await LeaveLobby(userId, token);
        var lobby = new Lobby
        {
            Id = Guid.NewGuid().ToString("D"),
            OwnerId = userId,
            LobbyMembers = new Dictionary<long, LobbyMember>
            {
                [userId] = new()
                {
                    UserId = userId,
                    UserName = user.Name,
                    TeamId = null,
                    LastSeen = _timeProvider.GetUtcNow(),
                    JoinedAt = _timeProvider.GetUtcNow()
                }
            },
            CreatedAt = _timeProvider.GetUtcNow(),
            GameSettings = gameSettings,
            NumberOfPlayers = gameSettings.Players.Length,
            GameId = null
        };
        
        await _lobbyRepository.CreateLobby(lobby, token);
        _logger.LogInformation("Lobby {LobbyId} created by user {UserId}", lobby.Id, userId);
        return lobby;
    }

    public async Task<Lobby> JoinLobby(string lobbyId, User user, CancellationToken token)
    {
        var userId = user.Id;
        var userLobby = await _lobbyRepository.GetLobbyByUser(userId, token);
        if (userLobby != null)
        {
            if (userLobby.Id == lobbyId)
            {
                return userLobby;
            }

            await LeaveLobby(userLobby, userId, token);
        }

        var lobby = await _lobbyRepository.GetLobbyInfo(lobbyId, false, token);
        if (lobby == null)
        {
            throw new LobbyNotFoundException();
        }

        if (lobby.LobbyMembers.Count >= lobby.NumberOfPlayers)
        {
            throw new LobbyIsFullException();
        }

        var lobbyMember = new LobbyMember
        {
            UserId = userId,
            UserName = user.Name,
            TeamId = null,
            LastSeen = _timeProvider.GetUtcNow(),
            JoinedAt = _timeProvider.GetUtcNow()
        };

        await _lobbyRepository.AddUserToLobby(lobbyId, lobbyMember, token);
        _logger.LogInformation("User {UserId} joined to {LobbyId} ", userId, lobbyId);
        return await _lobbyRepository.GetLobbyInfo(lobbyId, false, token) ?? throw new NotSupportedException("Unexpected NRE");
    }

    public async Task<Lobby> StartGame(string lobbyId, User user, CancellationToken token)
    {
        var userId = user.Id;
        var lobby = await _lobbyRepository.GetLobbyInfo(lobbyId, false, token);
        if (lobby == null)
        {
            throw new LobbyNotFoundException();
        }

        if (lobby.OwnerId != userId)
        {
            throw new UserIsNotLobbyOwnerException();
        }

        if (lobby.LobbyMembers.Values.Any(x => x.TeamId == null))
        {
            throw new AllLobbyMembersMustHaveTeamException();
        }

        // todo нужно доработать назначение игроков - расставить правильно игроков по позициям
        var game = await _gameService.StartGame(new StartGameModel
        {
            GameName = Guid.NewGuid().ToString("D"),
            Settings = lobby.GameSettings
        });

        await _lobbyRepository.RemoveUsersFromLobby(lobbyId, token);
        await _lobbyRepository.Close(lobbyId, _timeProvider.GetUtcNow(), game.GameName, token);
        _logger.LogInformation("User {UserId} created game {GameId} from lobby {LobbyId} ", userId, game.GameName, lobbyId);
        return await _lobbyRepository.GetLobbyInfo(lobbyId, true, token) ?? throw new NotSupportedException("Unexpected NRE");
    }
    
    public async Task KickPlayer(string lobbyId, User kickInitiator, long kickTarget, CancellationToken token)
    {
        throw new NotImplementedException();
    }
    
    public async Task AssignTeam(string lobbyId, User user, long assignFor, int teamId, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task LeaveLobby(long userId, CancellationToken token)
    {
        var userLobby = await _lobbyRepository.GetLobbyByUser(userId, token);
        if (userLobby == null)
        {
            return;
        }

        await LeaveLobby(userLobby, userId, token);
    }

    public async Task<Lobby> GetLobbyInfo(string lobbyId, User user, CancellationToken token)
    {
        var userId = user.Id;
        var lobby = await _lobbyRepository.GetLobbyInfo(lobbyId, true, token);
        if (lobby == null)
        {
            throw new LobbyNotFoundException();
        }

        if (!lobby.LobbyMembers.ContainsKey(userId))
        {
            if (lobby.ClosedAt == null)
            {
                throw new UserIsNotLobbyMemberException();
            }

            throw new LobbyNotFoundException();
        }
        
        if (lobby.ClosedAt != null)
        {
            if (lobby.GameId == null)
            {
                throw new LobbyIsClosedException();
            }
            
            _logger.LogInformation("User {UserId} navigated to game {GameId} from {LobbyId} ", userId, lobby.GameId, lobbyId);
            return lobby;
        }

        await _lobbyRepository.UpdateUserKeepAlive(lobbyId, userId, _timeProvider.GetUtcNow(), token);
        return lobby;
    }

    private async Task LeaveLobby(Lobby userLobby, long userId, CancellationToken token)
    {
        if (userLobby.OwnerId == userId)
        {
            await _lobbyRepository.RemoveUsersFromLobby(userLobby.Id, token);
            await _lobbyRepository.Close(userLobby.Id, _timeProvider.GetUtcNow(), null, token);
            _logger.LogInformation("User {UserId} closed his lobby {LobbyId} ", userId, userLobby.Id);
            return;
        }

        await _lobbyRepository.RemoveUserFromLobbies(userId, token);
        _logger.LogInformation("User {UserId} left lobby {LobbyId} ", userId, userLobby.Id);
    }
}