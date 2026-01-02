using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbaGIStory.Server.Identity;

namespace UrbaGIStory.Server.Data.Configurations;

/// <summary>
/// Entity Framework configuration for ApplicationUser.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Configure IsActive property
        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasComment("Indicates whether the user is active (not deactivated). When false, the user cannot log in.");

        // Configure DeactivatedAt property
        builder.Property(u => u.DeactivatedAt)
            .IsRequired(false)
            .HasComment("Date and time when the user was deactivated (null if active).");

        // Configure DeactivatedBy property
        builder.Property(u => u.DeactivatedBy)
            .IsRequired(false)
            .HasComment("ID of the administrator who deactivated the user (null if active).");

        // Configure RowVersion for optimistic concurrency control
        builder.Property(u => u.RowVersion)
            .IsRowVersion()
            .IsRequired()
            .HasComment("Row version used for optimistic concurrency control. Automatically updated by the database on each update.");
    }
}

