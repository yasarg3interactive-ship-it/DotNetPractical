namespace PTimeJobs.Domain.Analytics;

public sealed class UserPreference
{
    private UserPreference()
    {
    }

    public Guid PreferenceId { get; private set; }
    public Guid UserId { get; private set; }
    public string PreferenceScope { get; private set; } = string.Empty;
    public string Preferences { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static UserPreference Create(Guid userId, string preferenceScope, string preferences = "{}")
    {
        if (string.IsNullOrWhiteSpace(preferenceScope))
        {
            throw new InvalidOperationException("Preference scope is required.");
        }

        return new UserPreference
        {
            PreferenceId = Guid.NewGuid(),
            UserId = userId,
            PreferenceScope = preferenceScope,
            Preferences = preferences,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void UpdatePreferences(string preferences)
    {
        Preferences = preferences;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
