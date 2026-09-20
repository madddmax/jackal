using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data;

public class JackalDbContext(DbContextOptions<JackalDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users { get; set; }
    
    public DbSet<GameEntity> Games { get; set; }
    
    public DbSet<GamePlayerEntity> GamePlayers { get; set; }
    
    public DbSet<CacheEntryEntity> CacheEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Применяем все конфигурации IEntityTypeConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(JackalDbContext).Assembly);
    }
}