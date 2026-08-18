using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Locations;

public sealed class LocationsCommandService(ApplicationDbContext dbContext) : ILocationsCommandService
{
    public async Task<CountryResponse> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken = default)
    {
        var codeTaken = await dbContext.Countries
            .AsNoTracking()
            .AnyAsync(country => country.Iso2 == request.Iso2.ToUpper() || country.Iso3 == request.Iso3.ToUpper(), cancellationToken);

        if (codeTaken)
        {
            throw new InvalidOperationException("A country with this ISO2/ISO3 code already exists.");
        }

        var country = Country.Create(request.Iso2, request.Iso3, request.CountryName, request.PhoneCode);
        dbContext.Countries.Add(country);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CountryResponse(country.CountryId, country.Iso2, country.Iso3, country.CountryName, country.PhoneCode);
    }

    public async Task<StateResponse> CreateStateAsync(CreateStateRequest request, CancellationToken cancellationToken = default)
    {
        var countryExists = await dbContext.Countries
            .AsNoTracking()
            .AnyAsync(country => country.CountryId == request.CountryId, cancellationToken);

        if (!countryExists)
        {
            throw new InvalidOperationException("Country not found.");
        }

        var state = State.Create(request.CountryId, request.StateName, request.StateCode);
        dbContext.States.Add(state);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new StateResponse(state.StateId, state.CountryId, state.StateName, state.StateCode);
    }

    public async Task<CityResponse> CreateCityAsync(CreateCityRequest request, CancellationToken cancellationToken = default)
    {
        var stateExists = await dbContext.States
            .AsNoTracking()
            .AnyAsync(state => state.StateId == request.StateId, cancellationToken);

        if (!stateExists)
        {
            throw new InvalidOperationException("State not found.");
        }

        var city = City.Create(request.StateId, request.CityName);
        dbContext.Cities.Add(city);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CityResponse(city.CityId, city.StateId, city.CityName);
    }

    public async Task<AreaResponse> CreateAreaAsync(CreateAreaRequest request, CancellationToken cancellationToken = default)
    {
        var cityExists = await dbContext.Cities
            .AsNoTracking()
            .AnyAsync(city => city.CityId == request.CityId, cancellationToken);

        if (!cityExists)
        {
            throw new InvalidOperationException("City not found.");
        }

        var area = Area.Create(request.CityId, request.AreaName, request.PostalCode);
        dbContext.Areas.Add(area);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new AreaResponse(area.AreaId, area.CityId, area.AreaName, area.PostalCode);
    }

    public async Task<LocationResponse> CreateLocationAsync(CreateLocationRequest request, CancellationToken cancellationToken = default)
    {
        var location = Location.Create(
            request.CountryId,
            request.StateId,
            request.CityId,
            request.AreaId,
            request.AddressLine1,
            request.AddressLine2,
            request.Landmark,
            request.Latitude,
            request.Longitude,
            request.GooglePlaceId);

        dbContext.Locations.Add(location);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new LocationResponse(
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
            location.GooglePlaceId);
    }
}
