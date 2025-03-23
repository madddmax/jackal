using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackalWebHost2.Data.EntityConfigurations;

public class GameUserEntityConfiguration  : IEntityTypeConfiguration<GameUserEntity>
{
    public void Configure(EntityTypeBuilder<GameUserEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder
            .HasOne(b => b.Game)
            .WithMany(b => b.GameUsers)
            .HasForeignKey(b => b.GameId);
        
        builder
            .HasOne(b => b.User)
            .WithMany(b => b.GameUsers)
            .HasForeignKey(g => g.UserId);
    }
}