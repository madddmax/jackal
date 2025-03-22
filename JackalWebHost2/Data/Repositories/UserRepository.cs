using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.Interfaces;
using JackalWebHost2.Models;

namespace JackalWebHost2.Data.Repositories;

public class UserRepository(JackalDbContext jackalDbContext) : IUserRepository
{
    public async Task<User?> GetUser(long id, CancellationToken token)
    {
        var userEntity = await jackalDbContext.Users.FindAsync(new object [id], token);
        if (userEntity == null)
            return null;

        return new User
        {
            Id = userEntity.Id,
            Login = userEntity.Login
        };
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

        return new User
        {
            Id = userEntity.Id,
            Login = userEntity.Login
        };
    }
}