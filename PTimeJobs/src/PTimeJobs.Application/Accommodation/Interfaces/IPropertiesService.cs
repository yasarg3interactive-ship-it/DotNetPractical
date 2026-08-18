using PTimeJobs.Application.Accommodation.Dtos;

namespace PTimeJobs.Application.Accommodation.Interfaces;

public interface IPropertiesService
{
    Task<PropertyResponse?> GetByIdAsync(Guid propertyId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PropertyResponse>> GetByProviderAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default);

    Task<PropertyResponse> CreateAsync(CreatePropertyRequest request, CancellationToken cancellationToken = default);

    Task<PropertyResponse?> DeactivateAsync(Guid propertyId, CancellationToken cancellationToken = default);

    Task<PropertyResponse?> AddImageAsync(Guid propertyId, AddPropertyImageRequest request, CancellationToken cancellationToken = default);

    Task<PropertyResponse?> AddFacilityAsync(Guid propertyId, AddPropertyFacilityRequest request, CancellationToken cancellationToken = default);
}
