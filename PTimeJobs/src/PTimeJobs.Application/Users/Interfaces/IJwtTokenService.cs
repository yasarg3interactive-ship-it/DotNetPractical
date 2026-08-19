namespace PTimeJobs.Application.Users.Interfaces;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(Guid userId, string? email, IReadOnlyCollection<string> roleCodes);
}
