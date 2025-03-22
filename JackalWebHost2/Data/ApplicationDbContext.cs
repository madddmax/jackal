using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JackalWebHost2.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<GameEntity> Games { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}