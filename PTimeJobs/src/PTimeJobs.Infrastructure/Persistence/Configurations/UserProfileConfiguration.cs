using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("user_profiles");

        builder.HasKey(profile => profile.UserId);

        builder.Property(profile => profile.UserId).HasColumnName("user_id");
        builder.Property(profile => profile.FirstName).HasColumnName("first_name").HasMaxLength(100);
        builder.Property(profile => profile.LastName).HasColumnName("last_name").HasMaxLength(100);
        builder.Property(profile => profile.DisplayName).HasColumnName("display_name").HasMaxLength(160);
        builder.Property(profile => profile.DateOfBirth).HasColumnName("date_of_birth");
        builder.Property(profile => profile.Gender).HasColumnName("gender").HasMaxLength(40);
        builder.Property(profile => profile.ProfilePhotoUrl).HasColumnName("profile_photo_url");
        builder.Property(profile => profile.Bio).HasColumnName("bio");
        builder.Property(profile => profile.DefaultLocationId).HasColumnName("default_location_id");
        builder.Property(profile => profile.PreferredLanguage).HasColumnName("preferred_language").HasMaxLength(20);
        builder.Property(profile => profile.Timezone).HasColumnName("timezone").HasMaxLength(80);
        builder.Property(profile => profile.CreatedAt).HasColumnName("created_at");
        builder.Property(profile => profile.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<UserProfile>(profile => profile.UserId);
    }
}
