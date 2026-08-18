using PTimeJobs.Domain.Users;

namespace PTimeJobs.Domain.Accommodation;

public sealed class AccommodationProvider
{
    private AccommodationProvider()
    {
    }

    public Guid AccommodationProviderId { get; private set; }
    public Guid UserId { get; private set; }
    public string BusinessName { get; private set; } = string.Empty;
    public VerificationStatus VerificationStatus { get; private set; }
    public string? ContactNumber { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static AccommodationProvider Create(Guid userId, string businessName, string? contactNumber = null)
    {
        if (string.IsNullOrWhiteSpace(businessName))
        {
            throw new InvalidOperationException("Business name is required.");
        }

        return new AccommodationProvider
        {
            AccommodationProviderId = Guid.NewGuid(),
            UserId = userId,
            BusinessName = businessName,
            VerificationStatus = VerificationStatus.Pending,
            ContactNumber = contactNumber,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkVerified()
    {
        VerificationStatus = VerificationStatus.Verified;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
