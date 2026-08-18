using PTimeJobs.Domain.Users;

namespace PTimeJobs.Domain.Food;

public sealed class FoodProvider
{
    private FoodProvider()
    {
    }

    public Guid FoodProviderId { get; private set; }
    public Guid UserId { get; private set; }
    public string BusinessName { get; private set; } = string.Empty;
    public string ProviderType { get; private set; } = string.Empty;
    public VerificationStatus VerificationStatus { get; private set; }
    public Guid? LocationId { get; private set; }
    public decimal AverageRating { get; private set; }
    public int RatingCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static FoodProvider Create(Guid userId, string businessName, string providerType, Guid? locationId = null)
    {
        if (string.IsNullOrWhiteSpace(businessName))
        {
            throw new InvalidOperationException("Business name is required.");
        }

        if (string.IsNullOrWhiteSpace(providerType))
        {
            throw new InvalidOperationException("Provider type is required.");
        }

        return new FoodProvider
        {
            FoodProviderId = Guid.NewGuid(),
            UserId = userId,
            BusinessName = businessName,
            ProviderType = providerType,
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
