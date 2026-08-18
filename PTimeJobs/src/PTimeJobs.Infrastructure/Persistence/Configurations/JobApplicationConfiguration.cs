using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("job_applications");

        builder.HasKey(application => application.ApplicationId);

        builder.Property(application => application.ApplicationId).HasColumnName("application_id");
        builder.Property(application => application.JobId).HasColumnName("job_id");
        builder.Property(application => application.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(application => application.Status).HasColumnName("status").HasColumnType("application_status");
        builder.Property(application => application.CoverNote).HasColumnName("cover_note");
        builder.Property(application => application.ExpectedSalary).HasColumnName("expected_salary").HasColumnType("numeric(12,2)");
        builder.Property(application => application.AppliedAt).HasColumnName("applied_at");
        builder.Property(application => application.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(application => new { application.JobId, application.WorkerProfileId }).IsUnique();

        builder.HasOne<Job>().WithMany().HasForeignKey(application => application.JobId);
        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(application => application.WorkerProfileId);
    }
}
