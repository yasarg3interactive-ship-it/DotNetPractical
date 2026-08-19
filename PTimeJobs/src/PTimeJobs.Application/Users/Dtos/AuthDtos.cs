namespace PTimeJobs.Application.Users.Dtos;

/// <summary>RoleCode must match an existing role's code (e.g. "worker", "employer", "hostel_owner", "food_provider") — "admin" cannot be self-assigned.</summary>
public sealed record RegisterRequest(string? Email, string? MobileNumber, string Password, string RoleCode);

public sealed record LoginRequest(string EmailOrMobile, string Password, string? DeviceId);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record AuthResponse(
    string AccessToken,
    DateTimeOffset AccessTokenExpiresAt,
    string RefreshToken,
    DateTimeOffset RefreshTokenExpiresAt,
    UserSummaryResponse User);
