using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    
    public GameRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    public async Task<Game?> GetGame(string gameName)
    {
        var gameEntity = await _applicationDbContext.Games
            .Where(g => g.Code == gameName)
            .FirstOrDefaultAsync();
        
        return gameEntity != null ? JsonHelper.DeserializeWithType<Game>(gameEntity.GameData) : null;
    }

    public async Task CreateGame(string gameName, Game game)
    {
        var gameEntity = new GameEntity
        {
            Code = gameName,
            Created = DateTime.UtcNow,
            GameData = JsonHelper.SerializeWithType(game)
        };

        await _applicationDbContext.Games.AddAsync(gameEntity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UpdateGame(string gameName, Game game)
    {
        var gameEntity = await _applicationDbContext.Games
            .Where(g => g.Code == gameName)
            .FirstAsync();
        
        gameEntity.Updated = DateTime.UtcNow;
        gameEntity.TurnNumber = game.TurnNo;
        gameEntity.GameData = JsonHelper.SerializeWithType(game);
        
        await _applicationDbContext.SaveChangesAsync();
    }
}