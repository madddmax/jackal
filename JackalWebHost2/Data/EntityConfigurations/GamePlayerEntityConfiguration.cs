using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackalWebHost2.Data.EntityConfigurations;

public class GamePlayerEntityConfiguration  : IEntityTypeConfiguration<GamePlayerEntity>
{
    public void Configure(EntityTypeBuilder<GamePlayerEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();
        
        builder
            .HasOne(b => b.Game)
            .WithMany(b => b.GamePlayers)
            .HasForeignKey(b => b.GameId);
        
        builder
            .HasOne(b => b.User)
            .WithMany(b => b.GamePlayers)
            .HasForeignKey(g => g.UserId)
            .IsRequired(false);
    }
}