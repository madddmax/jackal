using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository(JackalDbContext jackalDbContext) : IGameRepository
{
    public async Task<long> CreateGame(long userId, Game game)
    {
        var gameEntity = new GameEntity
        {
            Created = DateTime.UtcNow
        };
        await jackalDbContext.Games.AddAsync(gameEntity);
        await jackalDbContext.SaveChangesAsync();

        var gameUserEntity = new GameUserEntity
        {
            GameId = gameEntity.Id,
            UserId = userId
        };
        await jackalDbContext.GameUsers.AddAsync(gameUserEntity);
        await jackalDbContext.SaveChangesAsync();

        return gameEntity.Id;
    }
}