using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models.Lobby;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost2.Data.Repositories;

public class InMemoryLobbyRepository : ILobbyRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public InMemoryLobbyRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
    }
    
    public Task CreateLobby(Lobby lobby, CancellationToken token)
    {
        _memoryCache.Set(GetLobbyKey(lobby.Id), lobby, _cacheEntryOptions);
        _memoryCache.Set(GetUserKey(lobby.OwnerId), lobby.Id, _cacheEntryOptions);
        return Task.CompletedTask;
    }
    
    public Task AddUserToLobby(string lobbyId, LobbyMember lobbyMember, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        lobby!.LobbyMembers[lobbyMember.UserId] = lobbyMember;
        return Task.CompletedTask;
    }
    
    public Task<Lobby?> GetLobbyByUser(long userId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<string>(GetUserKey(userId), out var lobbyId))
        {
            return Task.FromResult<Lobby?>(null);
        }
        
        return _memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId!), out var lobby) 
            ? Task.FromResult(lobby) 
            : Task.FromResult<Lobby?>(null);
    }
    
    public Task<Lobby?> GetLobbyInfo(string lobbyId, bool includeDeleted, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            return Task.FromResult<Lobby?>(null);
        }

        _memoryCache.TryGetValue<string>(GetUserKey(lobby!.OwnerId), out _);
        return Task.FromResult<Lobby?>(lobby);
    }
    
    public Task RemoveUserFromLobbies(long userId, CancellationToken token)
    {
        // TODO ничего не делаем
        return Task.CompletedTask;
    }
    
    public Task UpdateUserKeepAlive(string lobbyId, long userId, DateTimeOffset time, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        lobby!.LobbyMembers[userId].LastSeen = time;
        return Task.CompletedTask;
    }

    public Task Close(string lobbyId, DateTimeOffset time, string? gameId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        lobby!.ClosedAt = time;
        lobby.GameId = gameId;
        return Task.CompletedTask;
    }

    public Task RemoveUsersFromLobby(string lobbyId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }
        
        lobby!.LobbyMembers.Clear();
        return Task.CompletedTask;
    }
    
    private static string GetLobbyKey(string str) => "lobby:" + str;
    
    private static string GetUserKey(long num) => "lobby-owner:" + num;
}