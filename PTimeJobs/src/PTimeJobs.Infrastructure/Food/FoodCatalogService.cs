using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;
using PTimeJobs.Domain.Food;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Food;

public sealed class FoodCatalogService(ApplicationDbContext dbContext) : IFoodCatalogService
{
    public async Task<IReadOnlyCollection<FoodItemResponse>> GetItemsByProviderAsync(Guid foodProviderId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FoodItems
            .AsNoTracking()
            .Where(item => item.FoodProviderId == foodProviderId)
            .Select(item => new FoodItemResponse(item.FoodItemId, item.FoodProviderId, item.ItemName, item.Description, item.FoodType, item.Price, item.IsAvailable))
            .ToListAsync(cancellationToken);
    }

    public async Task<FoodItemResponse?> GetItemByIdAsync(Guid foodItemId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FoodItems
            .AsNoTracking()
            .Where(item => item.FoodItemId == foodItemId)
            .Select(item => new FoodItemResponse(item.FoodItemId, item.FoodProviderId, item.ItemName, item.Description, item.FoodType, item.Price, item.IsAvailable))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FoodItemResponse> CreateItemAsync(CreateFoodItemRequest request, CancellationToken cancellationToken = default)
    {
        var providerExists = await dbContext.FoodProviders
            .AsNoTracking()
            .AnyAsync(provider => provider.FoodProviderId == request.FoodProviderId, cancellationToken);

        if (!providerExists)
        {
            throw new InvalidOperationException("Food provider not found.");
        }

        var item = FoodItem.Create(request.FoodProviderId, request.ItemName, request.Price, request.Description, request.FoodType);
        dbContext.FoodItems.Add(item);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new FoodItemResponse(item.FoodItemId, item.FoodProviderId, item.ItemName, item.Description, item.FoodType, item.Price, item.IsAvailable);
    }

    public async Task<IReadOnlyCollection<FoodPlanResponse>> GetPlansByProviderAsync(Guid foodProviderId, CancellationToken cancellationToken = default)
    {
        var plans = await dbContext.FoodPlans
            .AsNoTracking()
            .Where(plan => plan.FoodProviderId == foodProviderId)
            .ToListAsync(cancellationToken);

        var responses = new List<FoodPlanResponse>();
        foreach (var plan in plans)
        {
            responses.Add(await BuildPlanResponseAsync(plan, cancellationToken));
        }

        return responses;
    }

    public async Task<FoodPlanResponse?> GetPlanByIdAsync(Guid foodPlanId, CancellationToken cancellationToken = default)
    {
        var plan = await dbContext.FoodPlans.AsNoTracking().FirstOrDefaultAsync(p => p.FoodPlanId == foodPlanId, cancellationToken);
        return plan is null ? null : await BuildPlanResponseAsync(plan, cancellationToken);
    }

    public async Task<FoodPlanResponse> CreatePlanAsync(CreateFoodPlanRequest request, CancellationToken cancellationToken = default)
    {
        var providerExists = await dbContext.FoodProviders
            .AsNoTracking()
            .AnyAsync(provider => provider.FoodProviderId == request.FoodProviderId, cancellationToken);

        if (!providerExists)
        {
            throw new InvalidOperationException("Food provider not found.");
        }

        var plan = FoodPlan.Create(
            request.FoodProviderId,
            request.PlanName,
            request.DurationDays,
            request.Price,
            request.MealsPerDay,
            request.Description);

        dbContext.FoodPlans.Add(plan);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await BuildPlanResponseAsync(plan, cancellationToken);
    }

    public async Task<FoodPlanResponse?> AddPlanItemAsync(Guid foodPlanId, AddFoodPlanItemRequest request, CancellationToken cancellationToken = default)
    {
        var plan = await dbContext.FoodPlans.AsNoTracking().FirstOrDefaultAsync(p => p.FoodPlanId == foodPlanId, cancellationToken);
        if (plan is null)
        {
            return null;
        }

        var itemExists = await dbContext.FoodItems.AsNoTracking().AnyAsync(item => item.FoodItemId == request.FoodItemId, cancellationToken);
        if (!itemExists)
        {
            throw new InvalidOperationException("Food item not found.");
        }

        var alreadyAdded = await dbContext.FoodPlanItems
            .AsNoTracking()
            .AnyAsync(
                pi => pi.FoodPlanId == foodPlanId && pi.FoodItemId == request.FoodItemId && pi.MealSlot == request.MealSlot,
                cancellationToken);

        if (!alreadyAdded)
        {
            dbContext.FoodPlanItems.Add(FoodPlanItem.Create(foodPlanId, request.FoodItemId, request.MealSlot));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return await BuildPlanResponseAsync(plan, cancellationToken);
    }

    private async Task<FoodPlanResponse> BuildPlanResponseAsync(FoodPlan plan, CancellationToken cancellationToken)
    {
        var items = await (
            from planItem in dbContext.FoodPlanItems.AsNoTracking()
            where planItem.FoodPlanId == plan.FoodPlanId
            join item in dbContext.FoodItems.AsNoTracking() on planItem.FoodItemId equals item.FoodItemId
            select new FoodPlanItemResponse(planItem.FoodPlanId, planItem.FoodItemId, item.ItemName, planItem.MealSlot))
            .ToListAsync(cancellationToken);

        return new FoodPlanResponse(
            plan.FoodPlanId,
            plan.FoodProviderId,
            plan.PlanName,
            plan.Description,
            plan.DurationDays,
            plan.Price,
            plan.MealsPerDay,
            plan.IsActive,
            items);
    }
}
