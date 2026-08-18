using PTimeJobs.Application.Billing.Dtos;

namespace PTimeJobs.Application.Billing.Interfaces;

public interface IBillingSubscriptionsService
{
    Task<BillingSubscriptionResponse?> GetByIdAsync(Guid billingSubscriptionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<BillingSubscriptionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<BillingSubscriptionResponse> CreateAsync(CreateBillingSubscriptionRequest request, CancellationToken cancellationToken = default);

    Task<BillingSubscriptionResponse?> ActivateAsync(Guid billingSubscriptionId, CancellationToken cancellationToken = default);

    Task<BillingSubscriptionResponse?> CancelAsync(Guid billingSubscriptionId, DateTimeOffset endsAt, CancellationToken cancellationToken = default);
}
