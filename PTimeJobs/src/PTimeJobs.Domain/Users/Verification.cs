namespace PTimeJobs.Domain.Users;

public sealed class Verification
{
    private Verification()
    {
    }

    public Guid VerificationId { get; private set; }
    public Guid UserId { get; private set; }
    public VerificationChannel Channel { get; private set; }
    public string TargetValue { get; private set; } = string.Empty;
    public string? TokenHash { get; private set; }
    public VerificationStatus Status { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }
    public DateTimeOffset? VerifiedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }
    public string Metadata { get; private set; } = "{}";

    public static Verification Create(
        Guid userId,
        VerificationChannel channel,
        string targetValue,
        string? tokenHash,
        DateTimeOffset? expiresAt)
    {
        if (string.IsNullOrWhiteSpace(targetValue))
        {
            throw new InvalidOperationException("Target value is required.");
        }

        return new Verification
        {
            VerificationId = Guid.NewGuid(),
            UserId = userId,
            Channel = channel,
            TargetValue = targetValue,
            TokenHash = tokenHash,
            Status = VerificationStatus.Pending,
            RequestedAt = DateTimeOffset.UtcNow,
            ExpiresAt = expiresAt
        };
    }

    public bool IsValid()
    {
        return Status == VerificationStatus.Pending
            && (!ExpiresAt.HasValue || ExpiresAt.Value > DateTimeOffset.UtcNow);
    }

    public void MarkVerified()
    {
        Status = VerificationStatus.Verified;
        VerifiedAt = DateTimeOffset.UtcNow;
    }

    public void MarkFailed()
    {
        Status = VerificationStatus.Failed;
    }

    public void MarkExpired()
    {
        Status = VerificationStatus.Expired;
    }

    public void Revoke()
    {
        Status = VerificationStatus.Revoked;
    }
}
