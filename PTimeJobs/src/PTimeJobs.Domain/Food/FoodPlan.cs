namespace PTimeJobs.Domain.Food;

public sealed class FoodPlan
{
    private FoodPlan()
    {
    }

    public Guid FoodPlanId { get; private set; }
    public Guid FoodProviderId { get; private set; }
    public string PlanName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public int DurationDays { get; private set; }
    public decimal Price { get; private set; }
    public int MealsPerDay { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static FoodPlan Create(
        Guid foodProviderId,
        string planName,
        int durationDays,
        decimal price,
        int mealsPerDay,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(planName))
        {
            throw new InvalidOperationException("Plan name is required.");
        }

        if (durationDays <= 0)
        {
            throw new InvalidOperationException("Duration days must be greater than zero.");
        }

        if (mealsPerDay <= 0)
        {
            throw new InvalidOperationException("Meals per day must be greater than zero.");
        }

        return new FoodPlan
        {
            FoodPlanId = Guid.NewGuid(),
            FoodProviderId = foodProviderId,
            PlanName = planName,
            Description = description,
            DurationDays = durationDays,
            Price = price,
            MealsPerDay = mealsPerDay,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
}
