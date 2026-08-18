using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Locations;

public sealed class LocationsQueryService(ApplicationDbContext dbContext) : ILocationsQueryService
{
    public async Task<IReadOnlyCollection<CountryResponse>> GetCountriesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Countries
            .AsNoTracking()
            .OrderBy(country => country.CountryName)
            .Select(country => new CountryResponse(country.CountryId, country.Iso2, country.Iso3, country.CountryName, country.PhoneCode))
            .ToListAsync(cancellationToken);
    }

    public async Task<CountryResponse?> GetCountryByIdAsync(Guid countryId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Countries
            .AsNoTracking()
            .Where(country => country.CountryId == countryId)
            .Select(country => new CountryResponse(country.CountryId, country.Iso2, country.Iso3, country.CountryName, country.PhoneCode))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<StateResponse>> GetStatesAsync(Guid? countryId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.States.AsNoTracking();

        if (countryId.HasValue)
        {
            query = query.Where(state => state.CountryId == countryId);
        }

        return await query
            .OrderBy(state => state.StateName)
            .Select(state => new StateResponse(state.StateId, state.CountryId, state.StateName, state.StateCode))
            .ToListAsync(cancellationToken);
    }

    public async Task<StateResponse?> GetStateByIdAsync(Guid stateId, CancellationToken cancellationToken = default)
    {
        return await dbContext.States
            .AsNoTracking()
            .Where(state => state.StateId == stateId)
            .Select(state => new StateResponse(state.StateId, state.CountryId, state.StateName, state.StateCode))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CityResponse>> GetCitiesAsync(Guid? stateId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Cities.AsNoTracking();

        if (stateId.HasValue)
        {
            query = query.Where(city => city.StateId == stateId);
        }

        return await query
            .OrderBy(city => city.CityName)
            .Select(city => new CityResponse(city.CityId, city.StateId, city.CityName))
            .ToListAsync(cancellationToken);
    }

    public async Task<CityResponse?> GetCityByIdAsync(Guid cityId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Where(city => city.CityId == cityId)
            .Select(city => new CityResponse(city.CityId, city.StateId, city.CityName))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<AreaResponse>> GetAreasAsync(Guid? cityId, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Areas.AsNoTracking();

        if (cityId.HasValue)
        {
            query = query.Where(area => area.CityId == cityId);
        }

        return await query
            .OrderBy(area => area.AreaName)
            .Select(area => new AreaResponse(area.AreaId, area.CityId, area.AreaName, area.PostalCode))
            .ToListAsync(cancellationToken);
    }

    public async Task<AreaResponse?> GetAreaByIdAsync(Guid areaId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Areas
            .AsNoTracking()
            .Where(area => area.AreaId == areaId)
            .Select(area => new AreaResponse(area.AreaId, area.CityId, area.AreaName, area.PostalCode))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<LocationResponse?> GetLocationByIdAsync(Guid locationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Locations
            .AsNoTracking()
            .Where(location => location.LocationId == locationId)
            .Select(location => new LocationResponse(
                location.LocationId,
                location.CountryId,
                location.StateId,
                location.CityId,
                location.AreaId,
                location.AddressLine1,
                location.AddressLine2,
                location.Landmark,
                location.Latitude,
                location.Longitude,
                location.GooglePlaceId))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
