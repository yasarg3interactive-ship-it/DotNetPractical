using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;
using PTimeJobs.Domain.Food;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Food;

public sealed class FoodSubscriptionsService(ApplicationDbContext dbContext) : IFoodSubscriptionsService
{
    public async Task<FoodSubscriptionResponse?> GetByIdAsync(Guid foodSubscriptionId, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.FoodSubscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.FoodSubscriptionId == foodSubscriptionId, cancellationToken);

        return subscription is null ? null : ToResponse(subscription);
    }

    public async Task<IReadOnlyCollection<FoodSubscriptionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var subscriptions = await dbContext.FoodSubscriptions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartDate)
            .ToListAsync(cancellationToken);

        return subscriptions.Select(ToResponse).ToList();
    }

    public async Task<FoodSubscriptionResponse> CreateAsync(CreateFoodSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        var planExists = await dbContext.FoodPlans.AsNoTracking().AnyAsync(plan => plan.FoodPlanId == request.FoodPlanId, cancellationToken);
        if (!planExists)
        {
            throw new InvalidOperationException("Food plan not found.");
        }

        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var subscription = FoodSubscription.Create(request.FoodPlanId, request.UserId, request.StartDate, request.DeliveryLocationId);
        dbContext.FoodSubscriptions.Add(subscription);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(subscription);
    }

    public async Task<FoodSubscriptionResponse?> ActivateAsync(Guid foodSubscriptionId, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.FoodSubscriptions
            .FirstOrDefaultAsync(s => s.FoodSubscriptionId == foodSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return null;
        }

        subscription.Activate();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(subscription);
    }

    public async Task<FoodSubscriptionResponse?> CancelAsync(Guid foodSubscriptionId, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        var subscription = await dbContext.FoodSubscriptions
            .FirstOrDefaultAsync(s => s.FoodSubscriptionId == foodSubscriptionId, cancellationToken);

        if (subscription is null)
        {
            return null;
        }

        subscription.Cancel(endDate);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(subscription);
    }

    private static FoodSubscriptionResponse ToResponse(FoodSubscription subscription) => new(
        subscription.FoodSubscriptionId,
        subscription.FoodPlanId,
        subscription.UserId,
        subscription.Status.ToString(),
        subscription.StartDate,
        subscription.EndDate,
        subscription.DeliveryLocationId);
}
