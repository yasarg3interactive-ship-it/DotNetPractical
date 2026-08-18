namespace PTimeJobs.Application.Users.Dtos;

public sealed record VerificationResponse(
    Guid VerificationId,
    Guid UserId,
    string Channel,
    string TargetValue,
    string Status,
    DateTimeOffset RequestedAt,
    DateTimeOffset? VerifiedAt,
    DateTimeOffset? ExpiresAt);

public sealed record CreateVerificationRequest(Guid UserId, string Channel, string TargetValue, string? TokenHash, DateTimeOffset? ExpiresAt);
