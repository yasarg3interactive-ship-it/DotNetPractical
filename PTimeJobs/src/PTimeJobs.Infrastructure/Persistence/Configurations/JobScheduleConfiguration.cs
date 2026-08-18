using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobScheduleConfiguration : IEntityTypeConfiguration<JobSchedule>
{
    public void Configure(EntityTypeBuilder<JobSchedule> builder)
    {
        builder.ToTable("job_schedules");

        builder.HasKey(schedule => schedule.JobScheduleId);

        builder.Property(schedule => schedule.JobScheduleId).HasColumnName("job_schedule_id");
        builder.Property(schedule => schedule.JobId).HasColumnName("job_id");
        builder.Property(schedule => schedule.DayOfWeek).HasColumnName("day_of_week");
        builder.Property(schedule => schedule.StartTime).HasColumnName("start_time");
        builder.Property(schedule => schedule.EndTime).HasColumnName("end_time");
        builder.Property(schedule => schedule.StartDate).HasColumnName("start_date");
        builder.Property(schedule => schedule.EndDate).HasColumnName("end_date");
        builder.Property(schedule => schedule.ShiftLabel).HasColumnName("shift_label").HasMaxLength(80);
        builder.Property(schedule => schedule.RequiredWorkers).HasColumnName("required_workers");

        builder.HasOne<Job>().WithMany().HasForeignKey(schedule => schedule.JobId);
    }
}
