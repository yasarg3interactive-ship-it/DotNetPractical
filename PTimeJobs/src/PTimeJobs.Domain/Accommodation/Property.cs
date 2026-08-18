namespace PTimeJobs.Domain.Accommodation;

public sealed class Property
{
    private Property()
    {
    }

    public Guid PropertyId { get; private set; }
    public Guid AccommodationProviderId { get; private set; }
    public string PropertyName { get; private set; } = string.Empty;
    public string PropertyType { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public Guid? LocationId { get; private set; }
    public decimal? Latitude { get; private set; }
    public decimal? Longitude { get; private set; }
    public string? AddressText { get; private set; }
    public bool IsActive { get; private set; }
    public decimal AverageRating { get; private set; }
    public int RatingCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Property Create(
        Guid accommodationProviderId,
        string propertyName,
        string propertyType,
        Guid? locationId = null,
        decimal? latitude = null,
        decimal? longitude = null,
        string? addressText = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new InvalidOperationException("Property name is required.");
        }

        if (string.IsNullOrWhiteSpace(propertyType))
        {
            throw new InvalidOperationException("Property type is required.");
        }

        return new Property
        {
            PropertyId = Guid.NewGuid(),
            AccommodationProviderId = accommodationProviderId,
            PropertyName = propertyName,
            PropertyType = propertyType,
            Description = description,
            LocationId = locationId,
            Latitude = latitude,
            Longitude = longitude,
            AddressText = addressText,
            IsActive = true,
            AverageRating = 0m,
            RatingCount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RecordRating(decimal newRating)
    {
        var totalScore = (AverageRating * RatingCount) + newRating;
        RatingCount++;
        AverageRating = Math.Round(totalScore / RatingCount, 2);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
