namespace PTimeJobs.Domain.Locations;

public sealed class State
{
    private readonly List<City> _cities = [];

    private State()
    {
    }

    public Guid StateId { get; private set; }
    public Guid CountryId { get; private set; }
    public string StateName { get; private set; } = string.Empty;
    public string? StateCode { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public static State Create(Guid countryId, string stateName, string? stateCode = null)
    {
        if (string.IsNullOrWhiteSpace(stateName))
        {
            throw new InvalidOperationException("State name is required.");
        }

        return new State
        {
            StateId = Guid.NewGuid(),
            CountryId = countryId,
            StateName = stateName,
            StateCode = stateCode,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
