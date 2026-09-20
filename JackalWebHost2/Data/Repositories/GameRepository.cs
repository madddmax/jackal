using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Exceptions;
using JackalWebHost2.Models.Map;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository(IDbContextFactory<JackalDbContext> contextFactory) : IGameRepository
{
    public async Task<long> CreateGame(long userId, Game game)
    {
        var gameEntity = new GameEntity
        {
            MapId = game.Board.Generator.MapId,
            TilesPackName = game.Board.Generator.TilesPackName,
            MapSize = game.Board.MapSize,
            GameMode = game.GameMode,
            CreatorUserId = userId,
            Created = DateTime.UtcNow
        };
        
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.Games.AddAsync(gameEntity);
        await context.SaveChangesAsync();

        foreach (var team in game.Board.Teams)
        {
            var gamePlayerEntity = new GamePlayerEntity
            {
                GameId = gameEntity.Id,
                TeamId = team.Id,
                UserId = team.UserId != 0 ? team.UserId : null,
                PlayerName = team.PlayerName,
                MapPositionId = (byte)MapUtils.ToMapPositionId(team.ShipPosition, game.Board.MapSize)
            };
            await context.GamePlayers.AddAsync(gamePlayerEntity);
        }

        await context.SaveChangesAsync();

        return gameEntity.Id;
    }
    
    public async Task UpdateGame(long gameId, Game game)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var gameEntity = await context.Games
            .Include(g => g.GamePlayers)
            .FirstOrDefaultAsync(g => g.Id == gameId);
        
        if (gameEntity == null)
            throw new GameNotFoundException();

        gameEntity.Updated = DateTime.UtcNow;
        gameEntity.TurnNumber = game.TurnNumber;
        gameEntity.GameOver = game.IsGameOver;

        var maxCoins = game.IsGameOver
            ? game.Board.Teams.Max(x => x.Coins)
            : 0;
        
        foreach (var playerEntity in gameEntity.GamePlayers)
        {
            playerEntity.Coins = game.Board.Teams[playerEntity.TeamId].Coins;
            playerEntity.Winner = game.IsGameOver && playerEntity.Coins == maxCoins;
        }
        
        await context.SaveChangesAsync();
    }
}