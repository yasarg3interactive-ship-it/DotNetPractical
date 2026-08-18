namespace PTimeJobs.Domain.Locations;

public sealed class Area
{
    private Area()
    {
    }

    public Guid AreaId { get; private set; }
    public Guid CityId { get; private set; }
    public string AreaName { get; private set; } = string.Empty;
    public string? PostalCode { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Area Create(Guid cityId, string areaName, string? postalCode = null)
    {
        if (string.IsNullOrWhiteSpace(areaName))
        {
            throw new InvalidOperationException("Area name is required.");
        }

        return new Area
        {
            AreaId = Guid.NewGuid(),
            CityId = cityId,
            AreaName = areaName,
            PostalCode = postalCode,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
