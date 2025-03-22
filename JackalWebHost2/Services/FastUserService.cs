using JackalWebHost2.Models;
using Microsoft.Extensions.Caching.Memory;

namespace JackalWebHost2.Services;

public class FastUserService : IFastUserService
{
    private static long Id;
    
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    public FastUserService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(24));
    }

    public async Task<User?> GetUser(long id, CancellationToken token)
    {
        return _memoryCache.TryGetValue<User>(GetKey(id), out var user)
            ? user
            : null;
    }

    public Task<User> CreateUser(string login, CancellationToken token)
    {
        var user = new User
        {
            Id = Interlocked.Increment(ref Id),
            Login = login
        };

        _memoryCache.Set(GetKey(user.Id), user, _cacheEntryOptions);
        return Task.FromResult(user);
    }
    
    private static string GetKey(long num) => "user:" + num;
}