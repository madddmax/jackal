using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost2.Data.Repositories;

public class InMemoryGameStateRepository : IGameStateRepository
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;
    
    public InMemoryGameStateRepository(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));
    }
    
    public async Task<GameState?> GetGameState(string gameName)
    {
        return _memoryCache.TryGetValue(gameName, out GameState? gameState) && gameState != null 
            ? gameState
            : null;
    }

    public Task CreateGameState(string gameName, GameState gameState)
    {
        _memoryCache.Set(gameName, gameState, _cacheEntryOptions);
        return Task.CompletedTask;
    }

    public Task UpdateGameState(string gameName, GameState gameState)
    {
        _memoryCache.Set(gameName, gameState, _cacheEntryOptions);
        return Task.CompletedTask;
    }
}