using PTimeJobs.Domain.Billing;

namespace PTimeJobs.Domain.Food;

public sealed class FoodSubscription
{
    private FoodSubscription()
    {
    }

    public Guid FoodSubscriptionId { get; private set; }
    public Guid FoodPlanId { get; private set; }
    public Guid UserId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public Guid? DeliveryLocationId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static FoodSubscription Create(Guid foodPlanId, Guid userId, DateOnly startDate, Guid? deliveryLocationId = null)
    {
        return new FoodSubscription
        {
            FoodSubscriptionId = Guid.NewGuid(),
            FoodPlanId = foodPlanId,
            UserId = userId,
            Status = SubscriptionStatus.Trialing,
            StartDate = startDate,
            DeliveryLocationId = deliveryLocationId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Activate()
    {
        Status = SubscriptionStatus.Active;
    }

    public void Cancel(DateOnly endDate)
    {
        Status = SubscriptionStatus.Cancelled;
        EndDate = endDate;
    }

    public void MarkExpired(DateOnly endDate)
    {
        Status = SubscriptionStatus.Expired;
        EndDate = endDate;
    }

    public void MarkPastDue()
    {
        Status = SubscriptionStatus.PastDue;
    }
}
