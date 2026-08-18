using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class UserBehaviorEventConfiguration : IEntityTypeConfiguration<UserBehaviorEvent>
{
    public void Configure(EntityTypeBuilder<UserBehaviorEvent> builder)
    {
        builder.ToTable("user_behavior_events");

        builder.HasKey(behaviorEvent => behaviorEvent.BehaviorEventId);

        builder.Property(behaviorEvent => behaviorEvent.BehaviorEventId).HasColumnName("behavior_event_id");
        builder.Property(behaviorEvent => behaviorEvent.UserId).HasColumnName("user_id");
        builder.Property(behaviorEvent => behaviorEvent.EventName).HasColumnName("event_name").HasMaxLength(120);
        builder.Property(behaviorEvent => behaviorEvent.EntityType).HasColumnName("entity_type").HasMaxLength(80);
        builder.Property(behaviorEvent => behaviorEvent.EntityId).HasColumnName("entity_id");
        builder.Property(behaviorEvent => behaviorEvent.EventProperties).HasColumnName("event_properties").HasColumnType("jsonb");
        builder.Property(behaviorEvent => behaviorEvent.OccurredAt).HasColumnName("occurred_at");

        builder.HasOne<User>().WithMany().HasForeignKey(behaviorEvent => behaviorEvent.UserId);
    }
}
