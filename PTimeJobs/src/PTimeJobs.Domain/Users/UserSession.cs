using System.Net;

namespace PTimeJobs.Domain.Users;

public sealed class UserSession
{
    private UserSession()
    {
    }

    public Guid SessionId { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshTokenHash { get; private set; } = string.Empty;
    public SessionStatus Status { get; private set; }
    public IPAddress? IpAddress { get; private set; }
    public string? UserAgent { get; private set; }
    public string? DeviceId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }

    public static UserSession Create(
        Guid userId,
        string refreshTokenHash,
        DateTimeOffset expiresAt,
        IPAddress? ipAddress,
        string? userAgent,
        string? deviceId)
    {
        return new UserSession
        {
            SessionId = Guid.NewGuid(),
            UserId = userId,
            RefreshTokenHash = refreshTokenHash,
            Status = SessionStatus.Active,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            DeviceId = deviceId,
            CreatedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt
        };
    }

    public bool IsValid()
    {
        return Status == SessionStatus.Active && ExpiresAt > DateTimeOffset.UtcNow;
    }

    public void Revoke()
    {
        Status = SessionStatus.Revoked;
        RevokedAt = DateTimeOffset.UtcNow;
    }
}
