using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JackalWebHost2.Data.Entities;

namespace JackalWebHost2.Data.EntityConfigurations;

public class CacheEntryEntityConfiguration : IEntityTypeConfiguration<CacheEntryEntity>
{
    public void Configure(EntityTypeBuilder<CacheEntryEntity> builder)
    {
        builder.ToTable("CacheEntries");

        builder.HasKey(e => e.ObjectId);

        builder.Property(e => e.ObjectId)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(e => e.Payload)
            .HasColumnName("Payload")
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(e => e.CreatorId)
            .HasColumnName("CreatorId")
            .IsRequired();

        builder.Property(e => e.CreatorName)
            .HasColumnName("CreatorName")
            .HasMaxLength(30);

        builder.Property(e => e.PlayersJson)
            .HasColumnName("Players")
            .HasColumnType("jsonb");

        builder.Property(e => e.TimeStamp)
            .HasColumnName("TimeStamp")
            .IsRequired();

        builder.Property(e => e.IsCompleted)
            .HasColumnName("IsCompleted")
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(e => e.Created)
            .HasColumnName("Created")
            .HasDefaultValueSql("NOW()")
            .IsRequired();
        
        builder.Property(e => e.Updated)
            .HasColumnName("Updated")
            .HasDefaultValueSql("NOW()")
            .IsRequired();

        builder.HasIndex(e => e.IsCompleted);
        
        builder.HasIndex(e => e.TimeStamp);
    }
}