using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FoodPlanItemConfiguration : IEntityTypeConfiguration<FoodPlanItem>
{
    public void Configure(EntityTypeBuilder<FoodPlanItem> builder)
    {
        builder.ToTable("food_plan_items");

        builder.HasKey(planItem => new { planItem.FoodPlanId, planItem.FoodItemId, planItem.MealSlot });

        builder.Property(planItem => planItem.FoodPlanId).HasColumnName("food_plan_id");
        builder.Property(planItem => planItem.FoodItemId).HasColumnName("food_item_id");
        builder.Property(planItem => planItem.MealSlot).HasColumnName("meal_slot").HasMaxLength(40);

        builder.HasOne<FoodPlan>().WithMany().HasForeignKey(planItem => planItem.FoodPlanId);
        builder.HasOne<FoodItem>().WithMany().HasForeignKey(planItem => planItem.FoodItemId);
    }
}
