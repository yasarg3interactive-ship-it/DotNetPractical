using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(log => log.AuditLogId);

        builder.Property(log => log.AuditLogId).HasColumnName("audit_log_id");
        builder.Property(log => log.ActorUserId).HasColumnName("actor_user_id");
        builder.Property(log => log.Action).HasColumnName("action").HasMaxLength(120);
        builder.Property(log => log.EntityType).HasColumnName("entity_type").HasMaxLength(80);
        builder.Property(log => log.EntityId).HasColumnName("entity_id");
        builder.Property(log => log.BeforeData).HasColumnName("before_data").HasColumnType("jsonb");
        builder.Property(log => log.AfterData).HasColumnName("after_data").HasColumnType("jsonb");
        builder.Property(log => log.IpAddress).HasColumnName("ip_address").HasColumnType("inet");
        builder.Property(log => log.UserAgent).HasColumnName("user_agent");
        builder.Property(log => log.CreatedAt).HasColumnName("created_at");

        builder.HasOne<User>().WithMany().HasForeignKey(log => log.ActorUserId);
    }
}
