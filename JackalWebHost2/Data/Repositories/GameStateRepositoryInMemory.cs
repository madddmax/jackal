using System.Collections.Concurrent;
using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;

namespace JackalWebHost2.Data.Repositories;

public class GameStateRepositoryInMemory : IGameStateRepository
{
    private readonly IMemoryCache _gamesMemoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    private bool _hasChanges;
    private readonly ConcurrentDictionary<long, GameCacheEntry> _gamesEntries;

    public GameStateRepositoryInMemory()
    {
        _gamesEntries = new ConcurrentDictionary<long, GameCacheEntry>();
        _gamesMemoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .RegisterPostEvictionCallback(callback: EvictionCallback);
    }

    private void EvictionCallback(object? key, object? value, EvictionReason reason, object? state)
    {
        if (key is not long gameId)
        {
            return;
        }
        
        if (reason is EvictionReason.None or EvictionReason.Replaced)
        {
            return;
        }
        
        if (_gamesEntries.TryRemove(gameId, out _))
        {
            _hasChanges = true;
        }
    }

    public bool HasGamesChanges()
    {
        return _hasChanges;
    }

    public void ResetGamesChanges()
    {
        _hasChanges = false;
    }

    [Obsolete("Заменить на GetGamesEntries")]
    public IList<long> GetAllKeys()
    {
        return _gamesEntries.Values.Select(it => it.GameId).ToList();
    }

    public IList<GameCacheEntry> GetGamesEntries()
    {
        return _gamesEntries.Values.ToList();
    }

    public Task<Game?> GetGame(long gameId)
    {
        return Task.FromResult(_gamesMemoryCache.TryGetValue(gameId, out Game? game) && game != null 
            ? game
            : null);
    }

    public Task CreateGame(User user, long gameId, Game game)
    {
        _gamesMemoryCache.Set(gameId, game, _cacheEntryOptions);
        if (_gamesEntries.TryAdd(gameId, new GameCacheEntry
            {
                GameId = gameId,
                Creator = new GameCacheEntryCreator
                {
                    Id = user.Id,
                    Name = user.Login
                },
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }))
        {
            _hasChanges = true;
        }
        return Task.CompletedTask;
    }

    public Task UpdateGame(long gameId, Game game)
    {
        _gamesMemoryCache.Set(gameId, game, _cacheEntryOptions);
        if (game.IsGameOver)
        {
            _gamesEntries.TryRemove(gameId, out _);
        }
        else if (_gamesEntries.TryGetValue(gameId, out GameCacheEntry? entry))
        {
            entry.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        _hasChanges = true;

        return Task.CompletedTask;
    }
}