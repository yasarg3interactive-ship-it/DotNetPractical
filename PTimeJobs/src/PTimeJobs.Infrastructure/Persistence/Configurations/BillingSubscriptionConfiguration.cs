using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class BillingSubscriptionConfiguration : IEntityTypeConfiguration<BillingSubscription>
{
    public void Configure(EntityTypeBuilder<BillingSubscription> builder)
    {
        builder.ToTable("billing_subscriptions");

        builder.HasKey(subscription => subscription.BillingSubscriptionId);

        builder.Property(subscription => subscription.BillingSubscriptionId).HasColumnName("billing_subscription_id");
        builder.Property(subscription => subscription.UserId).HasColumnName("user_id");
        builder.Property(subscription => subscription.PlanCode).HasColumnName("plan_code").HasMaxLength(100);
        builder.Property(subscription => subscription.Status)
            .HasColumnName("status")
            .HasColumnType("subscription_status");
        builder.Property(subscription => subscription.StartsAt).HasColumnName("starts_at");
        builder.Property(subscription => subscription.EndsAt).HasColumnName("ends_at");
        builder.Property(subscription => subscription.ProviderName).HasColumnName("provider_name").HasMaxLength(80);
        builder.Property(subscription => subscription.ProviderSubscriptionId).HasColumnName("provider_subscription_id").HasMaxLength(160);
        builder.Property(subscription => subscription.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(subscription => subscription.CreatedAt).HasColumnName("created_at");

        builder.HasOne<User>().WithMany().HasForeignKey(subscription => subscription.UserId);
    }
}
