using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data.Repositories;

public class UserRepository(JackalDbContext jackalDbContext) : IUserRepository
{
    public async Task<User?> GetUser(long id, CancellationToken token)
    {
        var userEntity = await jackalDbContext.Users.FindAsync(new object [id], token);
        return userEntity != null ? ToUser(userEntity) : null;
    }

    public async Task<User?> GetUser(string login, CancellationToken token)
    {
        var userEntity = await jackalDbContext.Users.FirstOrDefaultAsync(u => u.Login == login, token);
        return userEntity != null ? ToUser(userEntity) : null;
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
            Login = entity.Login
        };
}