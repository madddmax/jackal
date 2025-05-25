using System.Collections.Concurrent;
using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace JackalWebHost2.Data.Repositories;

public class StateRepositoryInMemory<T> : IStateRepository<T> where T : class, ICompletable
{
    private readonly IMemoryCache _memoryCache;
    private readonly MemoryCacheEntryOptions _cacheEntryOptions;

    private bool _hasChanges;
    private readonly ConcurrentDictionary<long, CacheEntry> _entries;

    public StateRepositoryInMemory()
    {
        _entries = new ConcurrentDictionary<long, CacheEntry>();
        _memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
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
        
        if (_entries.TryRemove(gameId, out _))
        {
            _hasChanges = true;
        }
    }

    public bool HasChanges()
    {
        return _hasChanges;
    }

    public void ResetChanges()
    {
        _hasChanges = false;
    }

    public IList<CacheEntry> GetEntries()
    {
        return _entries.Values.ToList();
    }

    public T? GetObject(long objectId)
    {
        return _memoryCache.TryGetValue(objectId, out T? value) ? value : null;
    }

    public void CreateObject(User user, long objectId, T value)
    {
        _memoryCache.Set(objectId, value, _cacheEntryOptions);
        if (_entries.TryAdd(objectId, new CacheEntry
            {
                ObjectId = objectId,
                Creator = new CacheEntryCreator
                {
                    Id = user.Id,
                    Name = user.Login
                },
                TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            }))
        {
            _hasChanges = true;
        }
    }

    public void UpdateObject(long objectId, T value)
    {
        _memoryCache.Set(objectId, value, _cacheEntryOptions);
        if (value.IsCompleted)
        {
            _entries.TryRemove(objectId, out _);
        }
        else if (_entries.TryGetValue(objectId, out CacheEntry? entry))
        {
            entry.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        _hasChanges = true;
    }
}