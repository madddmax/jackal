using Jackal.Core;
using JackalWebHost2.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost2.Data.Repositories;

public class GameStateRepositoryInMemory : IGameStateRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    
    public GameStateRepositoryInMemory(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
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
        return Task.CompletedTask;
    }

    public Task UpdateGame(long gameId, Game game)
    {
        _memoryCache.Set(GetKey(gameId), game, _cacheEntryOptions);
        return Task.CompletedTask;
    }

    private static string GetKey(long gameId) => $"game:{gameId}";
}