using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerExperienceConfiguration : IEntityTypeConfiguration<WorkerExperience>
{
    public void Configure(EntityTypeBuilder<WorkerExperience> builder)
    {
        builder.ToTable("worker_experience");

        builder.HasKey(experience => experience.ExperienceId);

        builder.Property(experience => experience.ExperienceId).HasColumnName("experience_id");
        builder.Property(experience => experience.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(experience => experience.CompanyName).HasColumnName("company_name").HasMaxLength(180);
        builder.Property(experience => experience.JobTitle).HasColumnName("job_title").HasMaxLength(160);
        builder.Property(experience => experience.EmploymentType)
            .HasColumnName("employment_type")
            .HasColumnType("employment_type");
        builder.Property(experience => experience.StartDate).HasColumnName("start_date");
        builder.Property(experience => experience.EndDate).HasColumnName("end_date");
        builder.Property(experience => experience.Description).HasColumnName("description");
        builder.Property(experience => experience.LocationId).HasColumnName("location_id");
        builder.Property(experience => experience.CreatedAt).HasColumnName("created_at");

        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(experience => experience.WorkerProfileId);
        builder.HasOne<Location>().WithMany().HasForeignKey(experience => experience.LocationId);
    }
}
