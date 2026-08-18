using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerEducationConfiguration : IEntityTypeConfiguration<WorkerEducation>
{
    public void Configure(EntityTypeBuilder<WorkerEducation> builder)
    {
        builder.ToTable("worker_education");

        builder.HasKey(education => education.EducationId);

        builder.Property(education => education.EducationId).HasColumnName("education_id");
        builder.Property(education => education.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(education => education.InstitutionName).HasColumnName("institution_name").HasMaxLength(180);
        builder.Property(education => education.Degree).HasColumnName("degree").HasMaxLength(160);
        builder.Property(education => education.FieldOfStudy).HasColumnName("field_of_study").HasMaxLength(160);
        builder.Property(education => education.StartYear).HasColumnName("start_year");
        builder.Property(education => education.EndYear).HasColumnName("end_year");
        builder.Property(education => education.IsCurrent).HasColumnName("is_current");
        builder.Property(education => education.CreatedAt).HasColumnName("created_at");

        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(education => education.WorkerProfileId);
    }
}
