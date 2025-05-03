using System.Collections.Concurrent;
using Jackal.Core;
using JackalWebHost2.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace JackalWebHost2.Data.Repositories;

public class GameStateRepositoryInMemory : IGameStateRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    private bool _hasChanges;
    private readonly ConcurrentDictionary<object, GameCacheEntry> _statEntries;

    public GameStateRepositoryInMemory()
    {
        _statEntries = new ConcurrentDictionary<object, GameCacheEntry>();
        _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
        _cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(1))
            .RegisterPostEvictionCallback(callback: EvictionCallback);
    }

    private void EvictionCallback(object? key, object? value, EvictionReason reason, object? state)
    {
        if (reason == EvictionReason.None || reason == EvictionReason.Replaced)
        {
            return;
        }

        if (key is string { Length: > 5 } gameId)
        {
            if (_statEntries.TryRemove(gameId.Substring(5), out var val))
            {
                _hasChanges = true;
            }

            Console.WriteLine();
            Console.WriteLine("/*****************************************************/");
            Console.WriteLine("/*  EvictionCallback: Cache with key {0} has expired.  */", key);
            Console.WriteLine("/*****************************************************/");
            Console.WriteLine();
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

    public IList<long> GetAllKeys()
    {
        return _statEntries.Values.Select(it => it.GameId).ToList();
    }

    public Task<Game?> GetGame(long gameId)
    {
        return Task.FromResult(_memoryCache.TryGetValue(GetKey(gameId), out Game? game) && game != null 
            ? game
            : null);
    }

    public Task CreateGame(long gameId, Game game)
    {
        _memoryCache.Set(GetKey(gameId), game, _cacheEntryOptions);
        if (_statEntries.TryAdd(gameId, new GameCacheEntry { GameId = gameId }))
        {
            _hasChanges = true;
        }
        return Task.CompletedTask;
    }

    public Task UpdateGame(long gameId, Game game)
    {
        _memoryCache.Set(GetKey(gameId), game, _cacheEntryOptions);
        return Task.CompletedTask;
    }

    private static string GetKey(long gameId) => $"game:{gameId}";


    private class GameCacheEntry
    {
        public long GameId { get; init; }
    }
}