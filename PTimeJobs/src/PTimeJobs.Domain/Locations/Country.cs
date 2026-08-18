namespace PTimeJobs.Domain.Locations;

public sealed class Country
{
    private readonly List<State> _states = [];

    private Country()
    {
    }

    public Guid CountryId { get; private set; }
    public string Iso2 { get; private set; } = string.Empty;
    public string Iso3 { get; private set; } = string.Empty;
    public string CountryName { get; private set; } = string.Empty;
    public string? PhoneCode { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<State> States => _states.AsReadOnly();

    public static Country Create(string iso2, string iso3, string countryName, string? phoneCode = null)
    {
        if (string.IsNullOrWhiteSpace(iso2) || iso2.Length != 2)
        {
            throw new InvalidOperationException("ISO2 code must be exactly 2 characters.");
        }

        if (string.IsNullOrWhiteSpace(iso3) || iso3.Length != 3)
        {
            throw new InvalidOperationException("ISO3 code must be exactly 3 characters.");
        }

        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new InvalidOperationException("Country name is required.");
        }

        return new Country
        {
            CountryId = Guid.NewGuid(),
            Iso2 = iso2.ToUpperInvariant(),
            Iso3 = iso3.ToUpperInvariant(),
            CountryName = countryName,
            PhoneCode = phoneCode,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
