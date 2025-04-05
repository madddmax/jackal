using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository(JackalDbContext jackalDbContext) : IGameRepository
{
    public async Task<long> CreateGame(Game game)
    {
        var gameEntity = new GameEntity
        {
            Created = DateTime.UtcNow
        };

        await jackalDbContext.Games.AddAsync(gameEntity);
        await jackalDbContext.SaveChangesAsync();

        return gameEntity.Id;
    }
}