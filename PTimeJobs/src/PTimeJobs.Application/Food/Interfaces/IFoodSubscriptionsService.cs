using PTimeJobs.Application.Food.Dtos;

namespace PTimeJobs.Application.Food.Interfaces;

public interface IFoodSubscriptionsService
{
    Task<FoodSubscriptionResponse?> GetByIdAsync(Guid foodSubscriptionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FoodSubscriptionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<FoodSubscriptionResponse> CreateAsync(CreateFoodSubscriptionRequest request, CancellationToken cancellationToken = default);

    Task<FoodSubscriptionResponse?> ActivateAsync(Guid foodSubscriptionId, CancellationToken cancellationToken = default);

    Task<FoodSubscriptionResponse?> CancelAsync(Guid foodSubscriptionId, DateOnly endDate, CancellationToken cancellationToken = default);
}
