namespace PTimeJobs.Application.Reviews.Dtos;

public sealed record ReviewResponse(
    Guid ReviewId,
    Guid ReviewerUserId,
    string TargetEntityType,
    Guid TargetEntityId,
    string? RelatedEntityType,
    Guid? RelatedEntityId,
    short Rating,
    string? ReviewText,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record CreateReviewRequest(
    Guid ReviewerUserId,
    string TargetEntityType,
    Guid TargetEntityId,
    short Rating,
    string? ReviewText,
    string? RelatedEntityType,
    Guid? RelatedEntityId);
