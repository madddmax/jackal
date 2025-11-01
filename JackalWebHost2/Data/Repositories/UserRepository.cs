using Jackal.Core.Players;
using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class UserRepository(JackalDbContext jackalDbContext) : IUserRepository
{
    public async Task<User?> GetUser(long id, CancellationToken token)
    {
        var userEntity = await jackalDbContext.Users
            .Include(u => u.GamePlayers)
            .FirstOrDefaultAsync(u => u.Id == id, token);
        
        return userEntity != null ? ToUser(userEntity) : null;
    }

    public async Task<User?> GetUser(string login, CancellationToken token)
    {
        var userEntity = await jackalDbContext.Users
            .Include(u => u.GamePlayers)
            .FirstOrDefaultAsync(
                u => u.Login.ToLower() == login.ToLower(),
                token
            );
        
        return userEntity != null ? ToUser(userEntity) : null;
    }

    public async Task<IList<User>> GetUsers(long[] ids, CancellationToken token)
    {
        var users = await jackalDbContext.Users
            .Include(u => u.GamePlayers)
            .Where(u => ids.Contains(u.Id)).ToListAsync(token);
        
        return users.Select(ToUser).ToList();
    }

    public async Task<User> CreateUser(string login, CancellationToken token)
    {
        var userEntity = new UserEntity
        {
            Login = login,
            Created = DateTime.UtcNow
        };

        await jackalDbContext.Users.AddAsync(userEntity, token);
        await jackalDbContext.SaveChangesAsync(token);

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