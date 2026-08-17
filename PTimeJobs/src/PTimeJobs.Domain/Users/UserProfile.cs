namespace PTimeJobs.Domain.Users;

public sealed class UserProfile
{
    private UserProfile()
    {
    }

    public Guid UserId { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? DisplayName { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public string? ProfilePhotoUrl { get; private set; }
    public string? Bio { get; private set; }
    public Guid? DefaultLocationId { get; private set; }
    public string PreferredLanguage { get; private set; } = "en";
    public string Timezone { get; private set; } = "Asia/Kolkata";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static UserProfile Create(Guid userId)
    {
        return new UserProfile
        {
            UserId = userId,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void UpdateDetails(
        string? firstName,
        string? lastName,
        string? displayName,
        DateOnly? dateOfBirth,
        string? gender,
        string? bio)
    {
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Bio = bio;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
