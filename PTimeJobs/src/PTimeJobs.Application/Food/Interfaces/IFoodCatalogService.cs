using PTimeJobs.Application.Food.Dtos;

namespace PTimeJobs.Application.Food.Interfaces;

public interface IFoodCatalogService
{
    Task<IReadOnlyCollection<FoodItemResponse>> GetItemsByProviderAsync(Guid foodProviderId, CancellationToken cancellationToken = default);

    Task<FoodItemResponse?> GetItemByIdAsync(Guid foodItemId, CancellationToken cancellationToken = default);

    Task<FoodItemResponse> CreateItemAsync(CreateFoodItemRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FoodPlanResponse>> GetPlansByProviderAsync(Guid foodProviderId, CancellationToken cancellationToken = default);

    Task<FoodPlanResponse?> GetPlanByIdAsync(Guid foodPlanId, CancellationToken cancellationToken = default);

    Task<FoodPlanResponse> CreatePlanAsync(CreateFoodPlanRequest request, CancellationToken cancellationToken = default);

    Task<FoodPlanResponse?> AddPlanItemAsync(Guid foodPlanId, AddFoodPlanItemRequest request, CancellationToken cancellationToken = default);
}
