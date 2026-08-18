using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Billing.Dtos;
using PTimeJobs.Application.Billing.Interfaces;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Billing;

public sealed class BillingSubscriptionsService(ApplicationDbContext dbContext) : IBillingSubscriptionsService
{
    public async Task<BillingSubscriptionResponse?> GetByIdAsync(Guid billingSubscriptionId, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.BillingSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.BillingSubscriptionId == billingSubscriptionId, cancellationToken);

        return subscription is null ? null : ToResponse(subscription);
    }

    public async Task<IReadOnlyCollection<BillingSubscriptionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await dbContext.BillingSubscriptions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);

        return subscriptions.Select(ToResponse).ToList();
    }

    public async Task<BillingSubscriptionResponse> CreateAsync(CreateBillingSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var subscription = BillingSubscription.Create(
            request.UserId,
            request.PlanCode,
            request.StartsAt,
            request.ProviderName,
            request.ProviderSubscriptionId);

        dbContext.BillingSubscriptions.Add(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(subscription);
    }

    public async Task<BillingSubscriptionResponse?> ActivateAsync(Guid billingSubscriptionId, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.BillingSubscriptions
            .FirstOrDefaultAsync(s => s.BillingSubscriptionId == billingSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return null;
        }

        subscription.Activate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(subscription);
    }

    public async Task<BillingSubscriptionResponse?> CancelAsync(Guid billingSubscriptionId, DateTimeOffset endsAt, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.BillingSubscriptions
            .FirstOrDefaultAsync(s => s.BillingSubscriptionId == billingSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return null;
        }

        subscription.Cancel(endsAt);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(subscription);
    }

    private static BillingSubscriptionResponse ToResponse(BillingSubscription subscription) => new(
        subscription.BillingSubscriptionId,
        subscription.UserId,
        subscription.PlanCode,
        subscription.Status.ToString(),
        subscription.StartsAt,
        subscription.EndsAt,
        subscription.ProviderName,
        subscription.CreatedAt);
}
