using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerAvailabilityConfiguration : IEntityTypeConfiguration<WorkerAvailability>
{
    public void Configure(EntityTypeBuilder<WorkerAvailability> builder)
    {
        builder.ToTable("worker_availability");

        builder.HasKey(availability => availability.AvailabilityId);

        builder.Property(availability => availability.AvailabilityId).HasColumnName("availability_id");
        builder.Property(availability => availability.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(availability => availability.DayOfWeek).HasColumnName("day_of_week");
        builder.Property(availability => availability.StartTime).HasColumnName("start_time");
        builder.Property(availability => availability.EndTime).HasColumnName("end_time");
        builder.Property(availability => availability.EffectiveFrom).HasColumnName("effective_from");
        builder.Property(availability => availability.EffectiveTo).HasColumnName("effective_to");
        builder.Property(availability => availability.IsAvailable).HasColumnName("is_available");
        builder.Property(availability => availability.CreatedAt).HasColumnName("created_at");

        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(availability => availability.WorkerProfileId);
    }
}
