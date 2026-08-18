namespace PTimeJobs.Domain.Analytics;

public sealed class RecommendationHistory
{
    private RecommendationHistory()
    {
    }

    public Guid RecommendationId { get; private set; }
    public Guid UserId { get; private set; }
    public string RecommendationType { get; private set; } = string.Empty;
    public string TargetEntityType { get; private set; } = string.Empty;
    public Guid TargetEntityId { get; private set; }
    public decimal? Score { get; private set; }
    public string? ModelVersion { get; private set; }
    public string Reason { get; private set; } = "{}";
    public DateTimeOffset ShownAt { get; private set; }
    public DateTimeOffset? ClickedAt { get; private set; }
    public DateTimeOffset? DismissedAt { get; private set; }
    public DateTimeOffset? ConvertedAt { get; private set; }

    public static RecommendationHistory Create(
        Guid userId,
        string recommendationType,
        string targetEntityType,
        Guid targetEntityId,
        decimal? score = null,
        string? modelVersion = null)
    {
        if (string.IsNullOrWhiteSpace(recommendationType))
        {
            throw new InvalidOperationException("Recommendation type is required.");
        }

        if (string.IsNullOrWhiteSpace(targetEntityType))
        {
            throw new InvalidOperationException("Target entity type is required.");
        }

        return new RecommendationHistory
        {
            RecommendationId = Guid.NewGuid(),
            UserId = userId,
            RecommendationType = recommendationType,
            TargetEntityType = targetEntityType,
            TargetEntityId = targetEntityId,
            Score = score,
            ModelVersion = modelVersion,
            ShownAt = DateTimeOffset.UtcNow
        };
    }

    public void RecordClick()
    {
        ClickedAt = DateTimeOffset.UtcNow;
    }

    public void RecordDismissal()
    {
        DismissedAt = DateTimeOffset.UtcNow;
    }

    public void RecordConversion()
    {
        ConvertedAt = DateTimeOffset.UtcNow;
    }
}
