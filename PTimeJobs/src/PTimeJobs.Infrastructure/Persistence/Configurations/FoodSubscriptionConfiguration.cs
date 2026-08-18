using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FoodSubscriptionConfiguration : IEntityTypeConfiguration<FoodSubscription>
{
    public void Configure(EntityTypeBuilder<FoodSubscription> builder)
    {
        builder.ToTable("food_subscriptions");

        builder.HasKey(subscription => subscription.FoodSubscriptionId);

        builder.Property(subscription => subscription.FoodSubscriptionId).HasColumnName("food_subscription_id");
        builder.Property(subscription => subscription.FoodPlanId).HasColumnName("food_plan_id");
        builder.Property(subscription => subscription.UserId).HasColumnName("user_id");
        builder.Property(subscription => subscription.Status)
            .HasColumnName("status")
            .HasColumnType("subscription_status");
        builder.Property(subscription => subscription.StartDate).HasColumnName("start_date");
        builder.Property(subscription => subscription.EndDate).HasColumnName("end_date");
        builder.Property(subscription => subscription.DeliveryLocationId).HasColumnName("delivery_location_id");
        builder.Property(subscription => subscription.CreatedAt).HasColumnName("created_at");

        builder.HasOne<FoodPlan>().WithMany().HasForeignKey(subscription => subscription.FoodPlanId);
        builder.HasOne<User>().WithMany().HasForeignKey(subscription => subscription.UserId);
        builder.HasOne<Location>().WithMany().HasForeignKey(subscription => subscription.DeliveryLocationId);
    }
}
