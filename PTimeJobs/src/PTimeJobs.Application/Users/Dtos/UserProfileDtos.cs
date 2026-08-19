namespace PTimeJobs.Application.Users.Dtos;

public sealed record UserProfileResponse(
    Guid UserId,
    string? FirstName,
    string? LastName,
    string? DisplayName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? ProfilePhotoUrl,
    string? Bio,
    Guid? DefaultLocationId,
    string PreferredLanguage,
    string Timezone,
    DateTimeOffset UpdatedAt);

public sealed record UpdateUserProfileRequest(string? FirstName, string? LastName, string? DisplayName, DateOnly? DateOfBirth, string? Gender, string? Bio);

public sealed record UserSessionResponse(
    Guid SessionId,
    Guid UserId,
    string Status,
    string? UserAgent,
    string? DeviceId,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt,
    DateTimeOffset? RevokedAt);
