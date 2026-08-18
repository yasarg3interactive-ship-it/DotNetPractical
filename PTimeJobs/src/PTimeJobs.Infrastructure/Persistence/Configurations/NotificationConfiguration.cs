using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Notifications;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(notification => notification.NotificationId);

        builder.Property(notification => notification.NotificationId).HasColumnName("notification_id");
        builder.Property(notification => notification.UserId).HasColumnName("user_id");
        builder.Property(notification => notification.NotificationType).HasColumnName("notification_type").HasMaxLength(100);
        builder.Property(notification => notification.Title).HasColumnName("title").HasMaxLength(180);
        builder.Property(notification => notification.Body).HasColumnName("body");
        builder.Property(notification => notification.Status)
            .HasColumnName("status")
            .HasColumnType("notification_status");
        builder.Property(notification => notification.EntityType).HasColumnName("entity_type").HasMaxLength(80);
        builder.Property(notification => notification.EntityId).HasColumnName("entity_id");
        builder.Property(notification => notification.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(notification => notification.CreatedAt).HasColumnName("created_at");
        builder.Property(notification => notification.SentAt).HasColumnName("sent_at");
        builder.Property(notification => notification.ReadAt).HasColumnName("read_at");

        builder.HasOne<User>().WithMany().HasForeignKey(notification => notification.UserId);
    }
}
