using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FoodPlanConfiguration : IEntityTypeConfiguration<FoodPlan>
{
    public void Configure(EntityTypeBuilder<FoodPlan> builder)
    {
        builder.ToTable("food_plans");

        builder.HasKey(plan => plan.FoodPlanId);

        builder.Property(plan => plan.FoodPlanId).HasColumnName("food_plan_id");
        builder.Property(plan => plan.FoodProviderId).HasColumnName("food_provider_id");
        builder.Property(plan => plan.PlanName).HasColumnName("plan_name").HasMaxLength(160);
        builder.Property(plan => plan.Description).HasColumnName("description");
        builder.Property(plan => plan.DurationDays).HasColumnName("duration_days");
        builder.Property(plan => plan.Price).HasColumnName("price").HasColumnType("numeric(12,2)");
        builder.Property(plan => plan.MealsPerDay).HasColumnName("meals_per_day");
        builder.Property(plan => plan.IsActive).HasColumnName("is_active");
        builder.Property(plan => plan.CreatedAt).HasColumnName("created_at");

        builder.HasOne<FoodProvider>().WithMany().HasForeignKey(plan => plan.FoodProviderId);
    }
}
