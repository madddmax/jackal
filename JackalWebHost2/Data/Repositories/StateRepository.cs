using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace JackalWebHost2.Data.Repositories;

public class StateRepository<T>(IDbContextFactory<JackalDbContext> contextFactory) : IStateRepository<T>
    where T : class, ICompletable
{
    // todo replace with JsonHelper
    private static readonly JsonSerializerSettings JsonSettings = new()
    {
        // ContractResolver = new DefaultContractResolver
        // {
        //     NamingStrategy = new CamelCaseNamingStrategy()
        // },
        TypeNameHandling = TypeNameHandling.Objects,
        // ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        // PreserveReferencesHandling = PreserveReferencesHandling.Objects
    };

    private bool _hasChanges = false;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

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
        using var context = contextFactory.CreateDbContext();

        return context.CacheEntries
            .Where(e => !e.IsCompleted)
            .Select(e => new CacheEntry
            {
                ObjectId = e.ObjectId,
                Creator = new CacheEntryUser
                {
                    Id = e.CreatorId,
                    Name = e.CreatorName
                },
                Players = e.PlayersJson != null 
                    ? Deserialize<CacheEntryUser[]>(e.PlayersJson) 
                    : null,
                TimeStamp = e.TimeStamp
            })
            .ToList();
    }

    public T? GetObject(long objectId)
    {
        using var context = contextFactory.CreateDbContext();

        var entity = context.CacheEntries
            .AsNoTracking()
            .FirstOrDefault(e => e.ObjectId == objectId && !e.IsCompleted);

        return entity?.Payload != null 
            ? Deserialize<T>(entity.Payload) 
            : null;
    }
    
    public void CreateObject(User creator, long objectId, T value, HashSet<User>? players)
    {
        _semaphore.Wait();
        
        try
        {
            using var context = contextFactory.CreateDbContext();
            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var innerContext = contextFactory.CreateDbContext();
                using var transaction = innerContext.Database.BeginTransaction();

                var existingEntity = innerContext.CacheEntries.Find(objectId);

                if (existingEntity == null)
                {
                    // Создаём новую запись
                    existingEntity = new CacheEntryEntity
                    {
                        ObjectId = objectId,
                        Payload = Serialize(value),
                        CreatorId = creator.Id,
                        CreatorName = creator.Login,
                        PlayersJson = players?.Count > 0
                            ? Serialize(players.Select(p => new CacheEntryUser
                            {
                                Id = p.Id,
                                Name = p.Login
                            }).ToArray())
                            : null,
                        TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        IsCompleted = value.IsCompleted,
                        Updated = DateTime.UtcNow
                    };

                    innerContext.CacheEntries.Add(existingEntity);
                }
                else
                {
                    // Обновляем существующую запись
                    existingEntity.Payload = Serialize(value);
                    existingEntity.PlayersJson = players?.Count > 0
                        ? Serialize(players.Select(p => new CacheEntryUser
                        {
                            Id = p.Id,
                            Name = p.Login
                        }).ToArray())
                        : existingEntity.PlayersJson;
                    existingEntity.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    existingEntity.IsCompleted = value.IsCompleted;
                    existingEntity.Updated = DateTime.UtcNow;
                }

                innerContext.SaveChanges();
                transaction.Commit();
            });

            _hasChanges = true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void UpdateObject(long objectId, T value, HashSet<User>? players)
    {
        _semaphore.Wait();
        
        try
        {
            using var context = contextFactory.CreateDbContext();
            var strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var innerContext = contextFactory.CreateDbContext();
                using var transaction = innerContext.Database.BeginTransaction();

                var entity = innerContext.CacheEntries.Find(objectId);

                if (entity == null)
                {
                    throw new InvalidOperationException($"Object with id {objectId} not found in repository");
                }

                if (value.IsCompleted)
                {
                    // Завершённый объект удаляется из активных
                    innerContext.CacheEntries.Remove(entity);
                }
                else
                {
                    entity.Payload = Serialize(value);

                    if (players?.Count > 0)
                    {
                        entity.PlayersJson = Serialize(players.Select(p => new CacheEntryUser
                        {
                            Id = p.Id,
                            Name = p.Login
                        }).ToArray());
                    }

                    entity.TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    entity.Updated = DateTime.UtcNow;
                }

                innerContext.SaveChanges();
                transaction.Commit();
            });

            _hasChanges = true;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private static string Serialize(object value) => JsonConvert.SerializeObject(value, JsonSettings);

    private static TValue Deserialize<TValue>(string json) => JsonConvert.DeserializeObject<TValue>(json, JsonSettings)!;
}