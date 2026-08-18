using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class HiringStatusHistoryConfiguration : IEntityTypeConfiguration<HiringStatusHistory>
{
    public void Configure(EntityTypeBuilder<HiringStatusHistory> builder)
    {
        builder.ToTable("hiring_status_history");

        builder.HasKey(history => history.HiringStatusHistoryId);

        builder.Property(history => history.HiringStatusHistoryId).HasColumnName("hiring_status_history_id");
        builder.Property(history => history.ApplicationId).HasColumnName("application_id");
        builder.Property(history => history.OldStatus).HasColumnName("old_status").HasColumnType("application_status");
        builder.Property(history => history.NewStatus).HasColumnName("new_status").HasColumnType("application_status");
        builder.Property(history => history.ChangedBy).HasColumnName("changed_by");
        builder.Property(history => history.Reason).HasColumnName("reason");
        builder.Property(history => history.CreatedAt).HasColumnName("created_at");

        builder.HasOne<JobApplication>().WithMany().HasForeignKey(history => history.ApplicationId);
        builder.HasOne<User>().WithMany().HasForeignKey(history => history.ChangedBy);
    }
}
