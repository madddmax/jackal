using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models.Lobby;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost2.Data.Repositories;

public class LobbyRepositoryInMemory : ILobbyRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public LobbyRepositoryInMemory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
    }
    
    public Task CreateLobby(Lobby lobby, CancellationToken token)
    {
        _memoryCache.Set(GetLobbyKey(lobby.Id), lobby, _cacheEntryOptions);
        _memoryCache.Set(GetLobbyOwnerKey(lobby.OwnerId), lobby.Id, _cacheEntryOptions);
        _memoryCache.Set(GetLobbyMemberKey(lobby.OwnerId), lobby.Id, _cacheEntryOptions);
        return Task.CompletedTask;
    }
    
    public Task AddUserToLobby(string lobbyId, LobbyMember lobbyMember, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        lobby!.LobbyMembers[lobbyMember.UserId] = lobbyMember;
        _memoryCache.Set(GetLobbyMemberKey(lobbyMember.UserId), lobby.Id, _cacheEntryOptions);
        return Task.CompletedTask;
    }
    
    public Task<Lobby?> GetLobbyByUser(long userId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<string>(GetLobbyOwnerKey(userId), out var lobbyId))
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

        // Обновляем TTL
        _memoryCache.TryGetValue<string>(GetLobbyOwnerKey(lobby!.OwnerId), out _);
        return Task.FromResult<Lobby?>(lobby);
    }
    
    public Task RemoveUserFromLobbies(long userId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<string>(GetLobbyMemberKey(userId), out var lobbyId))
        {
            return Task.CompletedTask;
        }
        
        _memoryCache.Remove(GetLobbyMemberKey(userId));
        _memoryCache.Remove(GetLobbyOwnerKey(userId));
        if (_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId!), out var lobby))
        {
            lobby!.LobbyMembers.Remove(userId);
        }
        
        return Task.CompletedTask;
    }
    
    public Task UpdateUserKeepAlive(string lobbyId, long userId, DateTimeOffset time, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        // Обновляем TTL
        _memoryCache.TryGetValue<string>(GetLobbyMemberKey(userId), out _);
        lobby!.LobbyMembers[userId].LastSeen = time;
        return Task.CompletedTask;
    }

    public Task Close(string lobbyId, DateTimeOffset time, long? gameId, long[]? gameMembers, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }

        lobby!.ClosedAt = time;
        lobby.GameId = gameId;
        lobby.GameMembers = gameMembers ?? [];
        return Task.CompletedTask;
    }

    public Task RemoveUsersFromLobby(string lobbyId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }
        
        _memoryCache.Remove(GetLobbyOwnerKey(lobby!.OwnerId));
        foreach (var member in lobby.LobbyMembers.Keys)
        {
            _memoryCache.Remove(GetLobbyMemberKey(member));
        }
        
        lobby!.LobbyMembers.Clear();
        return Task.CompletedTask;
    }
    
    public Task AssignTeam(string lobbyId, long userId, long? teamId, CancellationToken token)
    {
        if (!_memoryCache.TryGetValue<Lobby>(GetLobbyKey(lobbyId), out var lobby) )
        {
            throw new NotSupportedException();
        }
        
        lobby!.LobbyMembers[userId].TeamId = teamId;
        return Task.CompletedTask;
    }
    
    private static string GetLobbyKey(string str) => "lobby:" + str;
    private static string GetLobbyOwnerKey(long num) => "lobby-owner:" + num;
    private static string GetLobbyMemberKey(long num) => "lobby-member:" + num;
}