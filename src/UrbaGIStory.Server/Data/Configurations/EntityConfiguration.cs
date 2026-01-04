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

        // Configure geometry foreign keys (only ONE can be set at a time)
        builder.Property(e => e.GeoPointId)
            .IsRequired(false)
            .HasComment("Optional link to a point geometry. Only ONE geometry FK can be set.");

        builder.Property(e => e.GeoLineId)
            .IsRequired(false)
            .HasComment("Optional link to a line geometry. Only ONE geometry FK can be set.");

        builder.Property(e => e.GeoPolygonId)
            .IsRequired(false)
            .HasComment("Optional link to a polygon geometry. Only ONE geometry FK can be set.");

        // Configure relationships with ON DELETE SET NULL
        builder.HasOne(e => e.GeoPoint)
            .WithMany(g => g.Entities)
            .HasForeignKey(e => e.GeoPointId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.GeoLine)
            .WithMany(g => g.Entities)
            .HasForeignKey(e => e.GeoLineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.GeoPolygon)
            .WithMany(g => g.Entities)
            .HasForeignKey(e => e.GeoPolygonId)
            .OnDelete(DeleteBehavior.SetNull);

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

        builder.HasIndex(e => e.GeoPointId)
            .HasDatabaseName("IX_Entities_GeoPointId");

        builder.HasIndex(e => e.GeoLineId)
            .HasDatabaseName("IX_Entities_GeoLineId");

        builder.HasIndex(e => e.GeoPolygonId)
            .HasDatabaseName("IX_Entities_GeoPolygonId");

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

        // Check constraint: only ONE geometry FK can be set at a time
        // This is enforced at the database level
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_Entities_SingleGeometry",
            @"(
                (CASE WHEN ""GeoPointId"" IS NOT NULL THEN 1 ELSE 0 END) +
                (CASE WHEN ""GeoLineId"" IS NOT NULL THEN 1 ELSE 0 END) +
                (CASE WHEN ""GeoPolygonId"" IS NOT NULL THEN 1 ELSE 0 END)
            ) <= 1"));
    }
}

