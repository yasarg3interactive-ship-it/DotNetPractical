namespace PTimeJobs.Domain.Food;

public sealed class FoodPlanItem
{
    private FoodPlanItem()
    {
    }

    public Guid FoodPlanId { get; private set; }
    public Guid FoodItemId { get; private set; }
    public string MealSlot { get; private set; } = string.Empty;

    public static FoodPlanItem Create(Guid foodPlanId, Guid foodItemId, string mealSlot)
    {
        if (string.IsNullOrWhiteSpace(mealSlot))
        {
            throw new InvalidOperationException("Meal slot is required.");
        }

        return new FoodPlanItem
        {
            FoodPlanId = foodPlanId,
            FoodItemId = foodItemId,
            MealSlot = mealSlot
        };
    }
}
