namespace PTimeJobs.Domain.Billing;

public enum SubscriptionStatus
{
    Trialing = 0,
    Active = 1,
    PastDue = 2,
    Cancelled = 3,
    Expired = 4
}
