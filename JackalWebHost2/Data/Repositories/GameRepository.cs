using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly JackalDbContext _jackalDbContext;
    
    public GameRepository(JackalDbContext jackalDbContext)
    {
        _jackalDbContext = jackalDbContext;
    }

    public async Task CreateGame(string gameName, Game game)
    {
        var gameEntity = new GameEntity
        {
            Code = gameName,
            Created = DateTime.UtcNow
        };

        await _jackalDbContext.Games.AddAsync(gameEntity);
        await _jackalDbContext.SaveChangesAsync();
    }
}