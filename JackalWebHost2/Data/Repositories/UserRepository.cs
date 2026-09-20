using Jackal.Core.Players;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class UserRepository(IDbContextFactory<JackalDbContext> contextFactory) : IUserRepository
{
    public async Task<User?> GetUser(long id, CancellationToken token)
    {
        await using var context = await contextFactory.CreateDbContextAsync(token);
        var userEntity = await context.Users
            .Include(u => u.GamePlayers)
            .FirstOrDefaultAsync(u => u.Id == id, token);
        
        return userEntity != null ? ToUser(userEntity) : null;
    }

    public async Task<User?> GetUser(string login, CancellationToken token)
    {
        await using var context = await contextFactory.CreateDbContextAsync(token);
        var userEntity = await context.Users
            .Include(u => u.GamePlayers)
            .FirstOrDefaultAsync(
                u => u.Login.ToLower() == login.ToLower(),
                token
            );
        
        return userEntity != null ? ToUser(userEntity) : null;
    }

    public async Task<IList<User>> GetUsers(long[] ids, CancellationToken token)
    {
        await using var context = await contextFactory.CreateDbContextAsync(token);
        var users = await context.Users
            .Include(u => u.GamePlayers)
            .Where(u => ((IEnumerable<long>)ids).Contains(u.Id)).ToListAsync(token);
        
        return users.Select(ToUser).ToList();
    }

    public async Task<User> CreateUser(string login, CancellationToken token)
    {
        var userEntity = new UserEntity
        {
            Login = login,
            Created = DateTime.UtcNow,
            Games = [],
            GamePlayers = []
        };

        await using var context = await contextFactory.CreateDbContextAsync(token);
        await context.Users.AddAsync(userEntity, token);
        await context.SaveChangesAsync(token);

        return ToUser(userEntity);
    }

    private static User ToUser(UserEntity entity) =>
        new()
        {
            Id = entity.Id,
            Login = entity.Login,
            Rank = entity.GamePlayers.Count(p => p.Winner).GetHeroes2Rank()
        };
}