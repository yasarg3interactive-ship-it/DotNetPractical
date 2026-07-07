namespace PTimeJobs.Application.Users.Dtos;

public sealed record UserSummaryResponse(
    Guid UserId,
    string? Email,
    string? MobileNumber,
    string Status,
    bool IsEmailVerified,
    bool IsMobileVerified,
    IReadOnlyCollection<string> Roles,
    DateTimeOffset CreatedAt);
