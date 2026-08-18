namespace PTimeJobs.Domain.Billing;

public sealed class BillingSubscription
{
    private BillingSubscription()
    {
    }

    public Guid BillingSubscriptionId { get; private set; }
    public Guid UserId { get; private set; }
    public string PlanCode { get; private set; } = string.Empty;
    public SubscriptionStatus Status { get; private set; }
    public DateTimeOffset StartsAt { get; private set; }
    public DateTimeOffset? EndsAt { get; private set; }
    public string? ProviderName { get; private set; }
    public string? ProviderSubscriptionId { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }

    public static BillingSubscription Create(
        Guid userId,
        string planCode,
        DateTimeOffset startsAt,
        string? providerName = null,
        string? providerSubscriptionId = null)
    {
        if (string.IsNullOrWhiteSpace(planCode))
        {
            throw new InvalidOperationException("Plan code is required.");
        }

        return new BillingSubscription
        {
            BillingSubscriptionId = Guid.NewGuid(),
            UserId = userId,
            PlanCode = planCode,
            Status = SubscriptionStatus.Trialing,
            StartsAt = startsAt,
            ProviderName = providerName,
            ProviderSubscriptionId = providerSubscriptionId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Activate()
    {
        Status = SubscriptionStatus.Active;
    }

    public void MarkPastDue()
    {
        Status = SubscriptionStatus.PastDue;
    }

    public void Cancel(DateTimeOffset endsAt)
    {
        Status = SubscriptionStatus.Cancelled;
        EndsAt = endsAt;
    }

    public void Expire(DateTimeOffset endsAt)
    {
        Status = SubscriptionStatus.Expired;
        EndsAt = endsAt;
    }
}
