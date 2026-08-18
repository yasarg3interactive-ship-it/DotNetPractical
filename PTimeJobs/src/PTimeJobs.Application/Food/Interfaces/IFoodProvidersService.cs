using PTimeJobs.Application.Food.Dtos;

namespace PTimeJobs.Application.Food.Interfaces;

public interface IFoodProvidersService
{
    Task<FoodProviderResponse?> GetByIdAsync(Guid foodProviderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<FoodProviderResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<FoodProviderResponse> CreateAsync(CreateFoodProviderRequest request, CancellationToken cancellationToken = default);

    Task<FoodProviderResponse?> MarkVerifiedAsync(Guid foodProviderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<DeliveryAreaResponse>> GetDeliveryAreasAsync(Guid foodProviderId, CancellationToken cancellationToken = default);

    Task<DeliveryAreaResponse?> AddDeliveryAreaAsync(Guid foodProviderId, CreateDeliveryAreaRequest request, CancellationToken cancellationToken = default);
}
