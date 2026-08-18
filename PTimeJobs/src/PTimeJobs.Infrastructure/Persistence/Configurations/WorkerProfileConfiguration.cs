using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Users;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerProfileConfiguration : IEntityTypeConfiguration<WorkerProfile>
{
    public void Configure(EntityTypeBuilder<WorkerProfile> builder)
    {
        builder.ToTable("worker_profiles");

        builder.HasKey(profile => profile.WorkerProfileId);

        builder.Property(profile => profile.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(profile => profile.UserId).HasColumnName("user_id");
        builder.Property(profile => profile.Headline).HasColumnName("headline").HasMaxLength(180);
        builder.Property(profile => profile.ExpectedSalaryMin).HasColumnName("expected_salary_min").HasColumnType("numeric(12,2)");
        builder.Property(profile => profile.ExpectedSalaryMax).HasColumnName("expected_salary_max").HasColumnType("numeric(12,2)");
        builder.Property(profile => profile.ExpectedSalaryModel)
            .HasColumnName("expected_salary_model")
            .HasColumnType("salary_model");
        builder.Property(profile => profile.TotalExperienceMonths).HasColumnName("total_experience_months");
        builder.Property(profile => profile.CurrentLocationId).HasColumnName("current_location_id");
        builder.Property(profile => profile.ResumeUrl).HasColumnName("resume_url");
        builder.Property(profile => profile.ProfileStrengthScore).HasColumnName("profile_strength_score").HasColumnType("numeric(5,2)");
        builder.Property(profile => profile.AverageRating).HasColumnName("average_rating").HasColumnType("numeric(3,2)");
        builder.Property(profile => profile.RatingCount).HasColumnName("rating_count");
        builder.Property(profile => profile.MatchingMetadata).HasColumnName("matching_metadata").HasColumnType("jsonb");
        builder.Property(profile => profile.CreatedAt).HasColumnName("created_at");
        builder.Property(profile => profile.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(profile => profile.UserId).IsUnique();

        builder.HasOne<User>().WithOne().HasForeignKey<WorkerProfile>(profile => profile.UserId);
        builder.HasOne<Location>().WithMany().HasForeignKey(profile => profile.CurrentLocationId);

        builder.Navigation(profile => profile.Skills).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
