namespace PTimeJobs.Application.Billing.Dtos;

public sealed record CancelBillingSubscriptionRequest(DateTimeOffset EndsAt);
