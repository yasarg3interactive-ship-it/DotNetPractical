using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Employers;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class EmployerProfileConfiguration : IEntityTypeConfiguration<EmployerProfile>
{
    public void Configure(EntityTypeBuilder<EmployerProfile> builder)
    {
        builder.ToTable("employer_profiles");

        builder.HasKey(profile => profile.EmployerProfileId);

        builder.Property(profile => profile.EmployerProfileId).HasColumnName("employer_profile_id");
        builder.Property(profile => profile.UserId).HasColumnName("user_id");
        builder.Property(profile => profile.CompanyName).HasColumnName("company_name").HasMaxLength(180);
        builder.Property(profile => profile.BusinessType).HasColumnName("business_type").HasMaxLength(120);
        builder.Property(profile => profile.RegistrationNumber).HasColumnName("registration_number").HasMaxLength(120);
        builder.Property(profile => profile.VerificationStatus)
            .HasColumnName("verification_status")
            .HasColumnType("verification_status");
        builder.Property(profile => profile.LocationId).HasColumnName("location_id");
        builder.Property(profile => profile.AverageRating).HasColumnName("average_rating").HasColumnType("numeric(3,2)");
        builder.Property(profile => profile.RatingCount).HasColumnName("rating_count");
        builder.Property(profile => profile.CreatedAt).HasColumnName("created_at");
        builder.Property(profile => profile.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<User>().WithMany().HasForeignKey(profile => profile.UserId);
        builder.HasOne<Location>().WithMany().HasForeignKey(profile => profile.LocationId);
    }
}
