using PTimeJobs.Application.Locations.Dtos;

namespace PTimeJobs.Application.Locations.Interfaces;

public interface ILocationsQueryService
{
    Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync(CancellationToken cancellationToken = default);

    Task<CountryResponse?> GetCountryByIdAsync(Guid countryId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<StateResponse>> GetStatesAsync(Guid? countryId, CancellationToken cancellationToken = default);

    Task<StateResponse?> GetStateByIdAsync(Guid stateId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<CityResponse>> GetCitiesAsync(Guid? stateId, CancellationToken cancellationToken = default);

    Task<CityResponse?> GetCityByIdAsync(Guid cityId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<AreaResponse>> GetAreasAsync(Guid? cityId, CancellationToken cancellationToken = default);

    Task<AreaResponse?> GetAreaByIdAsync(Guid areaId, CancellationToken cancellationToken = default);

    Task<LocationResponse?> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default);
}
