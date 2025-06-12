using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackalWebHost2.Data.EntityConfigurations;

public class GameEntityConfiguration : IEntityTypeConfiguration<GameEntity>
{
    public void Configure(EntityTypeBuilder<GameEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .Property(b => b.TilesPackName)
            .HasMaxLength(30);
        
        builder
            .HasOne(b => b.CreatorUser)
            .WithMany(b => b.Games)
            .HasForeignKey(g => g.CreatorUserId);
    }
}