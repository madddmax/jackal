using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    
    public GameRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task CreateGame(string gameName, Game game)
    {
        var gameEntity = new GameEntity
        {
            Code = gameName,
            Created = DateTime.UtcNow
        };

        await _applicationDbContext.Games.AddAsync(gameEntity);
        await _applicationDbContext.SaveChangesAsync();
    }
}