using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class AnalyticsEventConfiguration : IEntityTypeConfiguration<AnalyticsEvent>
{
    public void Configure(EntityTypeBuilder<AnalyticsEvent> builder)
    {
        builder.ToTable("analytics_events");

        builder.HasKey(analyticsEvent => analyticsEvent.AnalyticsEventId);

        builder.Property(analyticsEvent => analyticsEvent.AnalyticsEventId).HasColumnName("analytics_event_id");
        builder.Property(analyticsEvent => analyticsEvent.UserId).HasColumnName("user_id");
        builder.Property(analyticsEvent => analyticsEvent.AnonymousId).HasColumnName("anonymous_id").HasMaxLength(120);
        builder.Property(analyticsEvent => analyticsEvent.EventName).HasColumnName("event_name").HasMaxLength(120);
        builder.Property(analyticsEvent => analyticsEvent.Source).HasColumnName("source").HasMaxLength(80);
        builder.Property(analyticsEvent => analyticsEvent.SessionId).HasColumnName("session_id");
        builder.Property(analyticsEvent => analyticsEvent.EntityType).HasColumnName("entity_type").HasMaxLength(80);
        builder.Property(analyticsEvent => analyticsEvent.EntityId).HasColumnName("entity_id");
        builder.Property(analyticsEvent => analyticsEvent.Properties).HasColumnName("properties").HasColumnType("jsonb");
        builder.Property(analyticsEvent => analyticsEvent.OccurredAt).HasColumnName("occurred_at");

        builder.HasOne<User>().WithMany().HasForeignKey(analyticsEvent => analyticsEvent.UserId);
    }
}
