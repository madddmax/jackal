using JackalWebHost2.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JackalWebHost2.Data.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

        builder
            .Property(b => b.Login)
            .IsRequired()
            .HasMaxLength(30);
        
        builder
            .HasIndex(b => b.Login)
            .IsUnique();
    }
}