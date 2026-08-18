using PTimeJobs.Application.Accommodation.Dtos;

namespace PTimeJobs.Application.Accommodation.Interfaces;

public interface IAccommodationProvidersService
{
    Task<AccommodationProviderResponse?> GetByIdAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AccommodationProviderResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<AccommodationProviderResponse> CreateAsync(CreateAccommodationProviderRequest request, CancellationToken cancellationToken = default);

    Task<AccommodationProviderResponse?> MarkVerifiedAsync(Guid accommodationProviderId, CancellationToken cancellationToken = default);
}
