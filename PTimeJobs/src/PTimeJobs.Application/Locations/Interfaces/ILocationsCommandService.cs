using PTimeJobs.Application.Locations.Dtos;

namespace PTimeJobs.Application.Locations.Interfaces;

public interface ILocationsCommandService
{
    Task<CountryResponse> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken = default);

    Task<StateResponse> CreateStateAsync(CreateStateRequest request, CancellationToken cancellationToken = default);

    Task<CityResponse> CreateCityAsync(CreateCityRequest request, CancellationToken cancellationToken = default);

    Task<AreaResponse> CreateAreaAsync(CreateAreaRequest request, CancellationToken cancellationToken = default);

    Task<LocationResponse> CreateLocationAsync(CreateLocationRequest request, CancellationToken cancellationToken = default);
}
