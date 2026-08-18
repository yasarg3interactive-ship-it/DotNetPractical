namespace PTimeJobs.Domain.Locations;

public sealed class Location
{
    private Location()
    {
    }

    public Guid LocationId { get; private set; }
    public Guid? CountryId { get; private set; }
    public Guid? StateId { get; private set; }
    public Guid? CityId { get; private set; }
    public Guid? AreaId { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? Landmark { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? GooglePlaceId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Location Create(
        Guid? countryId,
        Guid? stateId,
        Guid? cityId,
        Guid? areaId,
        string? addressLine1,
        string? addressLine2,
        string? landmark,
        decimal? latitude,
        decimal? longitude,
        string? googlePlaceId)
    {
        if (latitude.HasValue != longitude.HasValue)
        {
            throw new InvalidOperationException("Latitude and longitude must be provided together.");
        }

        if (latitude is < -90 or > 90)
        {
            throw new InvalidOperationException("Latitude must be between -90 and 90.");
        }

        if (longitude is < -180 or > 180)
        {
            throw new InvalidOperationException("Longitude must be between -180 and 180.");
        }

        return new Location
        {
            LocationId = Guid.NewGuid(),
            CountryId = countryId,
            StateId = stateId,
            CityId = cityId,
            AreaId = areaId,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            Landmark = landmark,
            Latitude = latitude,
            Longitude = longitude,
            GooglePlaceId = googlePlaceId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void UpdateAddress(string? addressLine1, string? addressLine2, string? landmark)
    {
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        Landmark = landmark;
    }
}
