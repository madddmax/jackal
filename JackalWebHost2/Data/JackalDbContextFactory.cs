using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JackalWebHost2.Data;

public class JackalDbContextFactory : IDesignTimeDbContextFactory<JackalDbContext>
{
    private const string ConnectionString = "Server=127.0.0.1;Port=5432;Database=jackal;User Id=postgres;Password=postgres;";
    
    public JackalDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JackalDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        return new JackalDbContext(optionsBuilder.Options);
    }
}