using PTimeJobs.Domain.Users;

namespace PTimeJobs.Domain.Employers;

public sealed class EmployerProfile
{
    private EmployerProfile()
    {
    }

    public Guid EmployerProfileId { get; private set; }
    public Guid UserId { get; private set; }
    public string CompanyName { get; private set; } = string.Empty;
    public string? BusinessType { get; private set; }
    public string? RegistrationNumber { get; private set; }
    public VerificationStatus VerificationStatus { get; private set; }
    public Guid? LocationId { get; private set; }
    public decimal AverageRating { get; private set; }
    public int RatingCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static EmployerProfile Create(
        Guid userId,
        string companyName,
        string? businessType = null,
        string? registrationNumber = null,
        Guid? locationId = null)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new InvalidOperationException("Company name is required.");
        }

        return new EmployerProfile
        {
            EmployerProfileId = Guid.NewGuid(),
            UserId = userId,
            CompanyName = companyName,
            BusinessType = businessType,
            RegistrationNumber = registrationNumber,
            VerificationStatus = VerificationStatus.Pending,
            LocationId = locationId,
            AverageRating = 0m,
            RatingCount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkVerified()
    {
        VerificationStatus = VerificationStatus.Verified;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RecordRating(decimal newRating)
    {
        var totalScore = (AverageRating * RatingCount) + newRating;
        RatingCount++;
        AverageRating = Math.Round(totalScore / RatingCount, 2);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
