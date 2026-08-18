using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Employers;
using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.ToTable("jobs");

        builder.HasKey(job => job.JobId);

        builder.Property(job => job.JobId).HasColumnName("job_id");
        builder.Property(job => job.EmployerProfileId).HasColumnName("employer_profile_id");
        builder.Property(job => job.JobCategoryId).HasColumnName("job_category_id");
        builder.Property(job => job.Title).HasColumnName("title").HasMaxLength(180);
        builder.Property(job => job.Description).HasColumnName("description");
        builder.Property(job => job.EmploymentType).HasColumnName("employment_type").HasColumnType("employment_type");
        builder.Property(job => job.SalaryModel).HasColumnName("salary_model").HasColumnType("salary_model");
        builder.Property(job => job.SalaryMin).HasColumnName("salary_min").HasColumnType("numeric(12,2)");
        builder.Property(job => job.SalaryMax).HasColumnName("salary_max").HasColumnType("numeric(12,2)");
        builder.Property(job => job.OpeningsCount).HasColumnName("openings_count");
        builder.Property(job => job.MinExperienceMonths).HasColumnName("min_experience_months");
        builder.Property(job => job.Status).HasColumnName("status").HasColumnType("job_status");
        builder.Property(job => job.ApplicationDeadline).HasColumnName("application_deadline");
        builder.Property(job => job.PublishedAt).HasColumnName("published_at");
        builder.Property(job => job.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(job => job.CreatedAt).HasColumnName("created_at");
        builder.Property(job => job.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<EmployerProfile>().WithMany().HasForeignKey(job => job.EmployerProfileId);
        builder.HasOne<JobCategory>().WithMany().HasForeignKey(job => job.JobCategoryId);
    }
}
