using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Permission.
/// </summary>
public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .HasComment("Unique identifier for the permission.");

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasComment("ID of the user who has this permission.");

        builder.Property(p => p.EntityId)
            .IsRequired()
            .HasComment("ID of the entity this permission applies to.");

        builder.Property(p => p.CanRead)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the user can read/view the entity.");

        builder.Property(p => p.CanWrite)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the user can write/edit the entity.");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasComment("Date and time when the permission was created (UTC).");

        builder.Property(p => p.CreatedBy)
            .IsRequired()
            .HasComment("ID of the Office Manager who created this permission.");

        builder.Property(p => p.UpdatedAt)
            .IsRequired(false)
            .HasComment("Date and time when the permission was last updated (UTC).");

        builder.Property(p => p.UpdatedBy)
            .IsRequired(false)
            .HasComment("ID of the Office Manager who last updated this permission.");

        // Indexes for performance
        builder.HasIndex(p => new { p.UserId, p.EntityId })
            .IsUnique()
            .HasDatabaseName("IX_Permissions_UserId_EntityId");

        builder.HasIndex(p => p.UserId)
            .HasDatabaseName("IX_Permissions_UserId");

        builder.HasIndex(p => p.EntityId)
            .HasDatabaseName("IX_Permissions_EntityId");

        // Foreign key to ApplicationUser
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

