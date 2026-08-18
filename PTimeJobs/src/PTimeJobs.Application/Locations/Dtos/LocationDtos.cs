namespace PTimeJobs.Application.Locations.Dtos;

public sealed record CountryResponse(Guid CountryId, string Iso2, string Iso3, string CountryName, string? PhoneCode);

public sealed record CreateCountryRequest(string Iso2, string Iso3, string CountryName, string? PhoneCode);

public sealed record StateResponse(Guid StateId, Guid CountryId, string StateName, string? StateCode);

public sealed record CreateStateRequest(Guid CountryId, string StateName, string? StateCode);

public sealed record CityResponse(Guid CityId, Guid StateId, string CityName);

public sealed record CreateCityRequest(Guid StateId, string CityName);

public sealed record AreaResponse(Guid AreaId, Guid CityId, string AreaName, string? PostalCode);

public sealed record CreateAreaRequest(Guid CityId, string AreaName, string? PostalCode);

public sealed record LocationResponse(
    Guid LocationId,
    Guid? CountryId,
    Guid? StateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressLine1,
    string? AddressLine2,
    string? Landmark,
    decimal? Latitude,
    decimal? Longitude,
    string? GooglePlaceId);

public sealed record CreateLocationRequest(
    Guid? CountryId,
    Guid? StateId,
    Guid? CityId,
    Guid? AreaId,
    string? AddressLine1,
    string? AddressLine2,
    string? Landmark,
    decimal? Latitude,
    decimal? Longitude,
    string? GooglePlaceId);
