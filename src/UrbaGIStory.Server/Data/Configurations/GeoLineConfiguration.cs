using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbaGIStory.Server.Models;

namespace UrbaGIStory.Server.Data.Configurations;

/// <summary>
/// Entity Framework configuration for GeoLine.
/// Configures the table as a QGIS-editable line layer.
/// </summary>
public class GeoLineConfiguration : IEntityTypeConfiguration<GeoLine>
{
    public void Configure(EntityTypeBuilder<GeoLine> builder)
    {
        builder.ToTable("geo_lines");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .HasDefaultValueSql("gen_random_uuid()")
            .HasComment("Unique identifier for the line geometry.");

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(255)
            .HasComment("Name of the geometry (visible in QGIS).");

        builder.Property(g => g.Description)
            .HasComment("Optional description of the geometry.");

        // Configure PostGIS LineString geometry with SRID 4326 (WGS84)
        builder.Property(g => g.Geometry)
            .HasColumnType("geometry(LineString, 4326)")
            .HasComment("LineString geometry in WGS84 (EPSG:4326).");

        builder.Property(g => g.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("NOW()")
            .HasComment("Date and time when the geometry was created (UTC).");

        builder.Property(g => g.UpdatedAt)
            .HasComment("Date and time when the geometry was last updated (UTC).");

        builder.Property(g => g.IsDeleted)
            .IsRequired()
            .HasDefaultValue(false)
            .HasComment("Whether the geometry is deleted (soft delete).");

        builder.Property(g => g.DeletedAt)
            .HasComment("Date and time when the geometry was deleted (null if not deleted).");

        // Spatial index for efficient geographic queries (GIST index)
        builder.HasIndex(g => g.Geometry)
            .HasMethod("GIST");

        // Index for filtering active (non-deleted) geometries
        builder.HasIndex(g => g.IsDeleted)
            .HasFilter("\"IsDeleted\" = false");

        // Query filter to exclude soft-deleted geometries by default
        builder.HasQueryFilter(g => !g.IsDeleted);
    }
}

