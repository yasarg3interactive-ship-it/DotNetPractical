namespace PTimeJobs.Domain.Reviews;

public sealed class Review
{
    private Review()
    {
    }

    public Guid ReviewId { get; private set; }
    public Guid ReviewerUserId { get; private set; }
    public string TargetEntityType { get; private set; } = string.Empty;
    public Guid TargetEntityId { get; private set; }
    public string? RelatedEntityType { get; private set; }
    public Guid? RelatedEntityId { get; private set; }
    public short Rating { get; private set; }
    public string? ReviewText { get; private set; }
    public ReviewStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Review Create(
        Guid reviewerUserId,
        string targetEntityType,
        Guid targetEntityId,
        short rating,
        string? reviewText = null,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null)
    {
        if (string.IsNullOrWhiteSpace(targetEntityType))
        {
            throw new InvalidOperationException("Target entity type is required.");
        }

        if (rating is < 1 or > 5)
        {
            throw new InvalidOperationException("Rating must be between 1 and 5.");
        }

        return new Review
        {
            ReviewId = Guid.NewGuid(),
            ReviewerUserId = reviewerUserId,
            TargetEntityType = targetEntityType,
            TargetEntityId = targetEntityId,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
            Rating = rating,
            ReviewText = reviewText,
            Status = ReviewStatus.Published,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Flag()
    {
        Status = ReviewStatus.Flagged;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Hide()
    {
        Status = ReviewStatus.Hidden;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Remove()
    {
        Status = ReviewStatus.Removed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
