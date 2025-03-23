using JackalWebHost2.Data.Entities;
using JackalWebHost2.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data;

public class JackalDbContext(DbContextOptions<JackalDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<GameEntity> Games { get; set; }
    
    public DbSet<GameUserEntity> GameUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameEntityConfiguration());
        modelBuilder.ApplyConfiguration(new GameUserEntityConfiguration());
    }
}