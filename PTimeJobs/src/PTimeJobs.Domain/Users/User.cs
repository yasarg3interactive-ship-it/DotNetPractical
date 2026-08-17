namespace PTimeJobs.Domain.Users;

public sealed class User
{
    private readonly List<UserRole> _userRoles = [];

    private User()
    {
    }

    public Guid UserId { get; private set; }
    public string? Email { get; private set; }
    public string? MobileNumber { get; private set; }
    public string? PasswordHash { get; private set; }
    public AccountStatus Status { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public bool IsMobileVerified { get; private set; }
    public DateTimeOffset? LastLoginAt { get; private set; }
    public DateTimeOffset? LastActiveAt { get; private set; }
    public int FailedLoginCount { get; private set; }
    public DateTimeOffset? LockedUntil { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    public static User Create(string? email, string? mobileNumber, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(mobileNumber))
        {
            throw new InvalidOperationException("Email or mobile number is required.");
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new InvalidOperationException("Password hash is required.");
        }

        return new User
        {
            UserId = Guid.NewGuid(),
            Email = email,
            MobileNumber = mobileNumber,
            PasswordHash = passwordHash,
            Status = AccountStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkEmailVerified()
    {
        IsEmailVerified = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkMobileVerified()
    {
        IsMobileVerified = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        Status = AccountStatus.Active;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Suspend()
    {
        Status = AccountStatus.Suspended;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public bool IsLockedOut()
    {
        return LockedUntil.HasValue && LockedUntil.Value > DateTimeOffset.UtcNow;
    }

    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTimeOffset.UtcNow;
        LastActiveAt = DateTimeOffset.UtcNow;
        FailedLoginCount = 0;
        LockedUntil = null;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RecordFailedLogin()
    {
        FailedLoginCount++;
        if (FailedLoginCount >= 5)
        {
            LockedUntil = DateTimeOffset.UtcNow.AddMinutes(15);
        }

        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
