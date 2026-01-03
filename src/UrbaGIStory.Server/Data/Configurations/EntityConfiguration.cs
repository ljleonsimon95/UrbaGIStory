using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Entity.
/// </summary>
public class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
    public void Configure(EntityTypeBuilder<Entity> builder)
    {
        builder.ToTable("Entities");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .IsRequired()
            .HasComment("Unique identifier for the entity.");

        builder.Property(e => e.EntityType)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("Type of entity (building, street, plaza, etc.).");

        builder.Property(e => e.QGISGeometryId)
            .IsRequired(false)
            .HasComment("Optional link to QGIS geometry. Null if entity has no spatial representation.");

        // Configure JSONB column for dynamic properties
        builder.Property(e => e.DynamicProperties)
            .HasColumnType("jsonb")
            .IsRequired(false)
            .HasComment("Dynamic properties stored as JSONB. Properties are defined by categories assigned to the entity type.");

        // Configure RowVersion for optimistic concurrency
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .IsRequired()
            .HasComment("Row version used for optimistic concurrency control. Automatically updated by the database on each update.");

        // Configure soft delete
        builder.Property(e => e.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the entity is deleted (soft delete).");

        builder.Property(e => e.DeletedAt)
            .IsRequired(false)
            .HasComment("Date and time when the entity was deleted (null if not deleted).");

        builder.Property(e => e.DeletedBy)
            .IsRequired(false)
            .HasComment("ID of the user who deleted the entity (null if not deleted).");

        // Configure audit fields
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasComment("Date and time when the entity was created (UTC).");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasComment("ID of the user who created the entity.");

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false)
            .HasComment("Date and time when the entity was last updated (UTC).");

        builder.Property(e => e.UpdatedBy)
            .IsRequired(false)
            .HasComment("ID of the user who last updated the entity.");

        // Indexes for performance
        builder.HasIndex(e => e.EntityType)
            .HasDatabaseName("IX_Entities_EntityType");

        builder.HasIndex(e => e.QGISGeometryId)
            .HasDatabaseName("IX_Entities_QGISGeometryId");

        builder.HasIndex(e => e.IsDeleted)
            .HasDatabaseName("IX_Entities_IsDeleted");

        builder.HasIndex(e => e.CreatedBy)
            .HasDatabaseName("IX_Entities_CreatedBy");

        // GIN index on JSONB column for efficient queries
        builder.HasIndex(e => e.DynamicProperties)
            .HasMethod("gin")
            .HasDatabaseName("IX_Entities_DynamicProperties");

        // Filter out deleted entities by default
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}

