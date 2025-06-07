using Jackal.Core;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models.Map;

namespace JackalWebHost2.Data.Repositories;

public class GameRepository(JackalDbContext jackalDbContext) : IGameRepository
{
    public async Task<long> CreateGame(long userId, Game game)
    {
        var gameEntity = new GameEntity
        {
            CreatorUserId = userId,
            Created = DateTime.UtcNow
        };
        await jackalDbContext.Games.AddAsync(gameEntity);
        await jackalDbContext.SaveChangesAsync();

        foreach (var team in game.Board.Teams)
        {
            var gamePlayerEntity = new GamePlayerEntity
            {
                GameId = gameEntity.Id,
                UserId = team.UserId != 0 ? team.UserId : null,
                PlayerName = team.Name,
                MapPositionId = (byte)MapUtils.ToMapPositionId(team.ShipPosition, game.Board.MapSize)
            };
            await jackalDbContext.GamePlayers.AddAsync(gamePlayerEntity);
        }

        await jackalDbContext.SaveChangesAsync();

        return gameEntity.Id;
    }
}