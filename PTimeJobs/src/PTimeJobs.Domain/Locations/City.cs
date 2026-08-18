namespace PTimeJobs.Domain.Locations;

public sealed class City
{
    private readonly List<Area> _areas = [];

    private City()
    {
    }

    public Guid CityId { get; private set; }
    public Guid StateId { get; private set; }
    public string CityName { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<Area> Areas => _areas.AsReadOnly();

    public static City Create(Guid stateId, string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            throw new InvalidOperationException("City name is required.");
        }

        return new City
        {
            CityId = Guid.NewGuid(),
            StateId = stateId,
            CityName = cityName,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
