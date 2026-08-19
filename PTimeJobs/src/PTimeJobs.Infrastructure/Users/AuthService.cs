using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class AuthService(
    ApplicationDbContext dbContext,
    IPasswordHasher passwordHasher,
    IJwtTokenService jwtTokenService) : IAuthService
{
    private static readonly TimeSpan RefreshTokenLifetime = TimeSpan.FromDays(30);

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Email) && string.IsNullOrWhiteSpace(request.MobileNumber))
        {
            throw new InvalidOperationException("Email or mobile number is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            throw new InvalidOperationException("Password must be at least 8 characters.");
        }

        var emailTaken = !string.IsNullOrWhiteSpace(request.Email)
            && await dbContext.Users.AsNoTracking().AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailTaken)
        {
            throw new InvalidOperationException("This email is already registered.");
        }

        var mobileTaken = !string.IsNullOrWhiteSpace(request.MobileNumber)
            && await dbContext.Users.AsNoTracking().AnyAsync(u => u.MobileNumber == request.MobileNumber, cancellationToken);

        if (mobileTaken)
        {
            throw new InvalidOperationException("This mobile number is already registered.");
        }

        if (string.Equals(request.RoleCode, "admin", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("The admin role cannot be self-assigned during registration.");
        }

        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.RoleCode == request.RoleCode, cancellationToken);
        if (role is null)
        {
            throw new InvalidOperationException($"Unknown role '{request.RoleCode}'.");
        }

        var passwordHash = passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, request.MobileNumber, passwordHash);
        dbContext.Users.Add(user);
        dbContext.UserRoles.Add(UserRole.Create(user.UserId, role.RoleId));

        await dbContext.SaveChangesAsync(cancellationToken);

        return await IssueTokensAsync(user, [role.RoleCode], null, null, null, cancellationToken);
    }

    public async Task<AuthResponse> LoginAsync(
        LoginRequest request,
        string? ipAddress,
        string? userAgent,
        CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.EmailOrMobile || u.MobileNumber == request.EmailOrMobile, cancellationToken);

        if (user is null || string.IsNullOrEmpty(user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        if (user.IsLockedOut())
        {
            throw new InvalidOperationException("Account is temporarily locked due to too many failed login attempts.");
        }

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            user.RecordFailedLogin();
            await dbContext.SaveChangesAsync(cancellationToken);
            throw new InvalidOperationException("Invalid credentials.");
        }

        user.RecordSuccessfulLogin();
        await dbContext.SaveChangesAsync(cancellationToken);

        var roleCodes = await GetRoleCodesAsync(user.UserId, cancellationToken);

        IPAddress? parsedIp = null;
        if (!string.IsNullOrWhiteSpace(ipAddress))
        {
            IPAddress.TryParse(ipAddress, out parsedIp);
        }

        return await IssueTokensAsync(user, roleCodes, parsedIp, userAgent, request.DeviceId, cancellationToken);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(request.RefreshToken);

        var session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.RefreshTokenHash == tokenHash, cancellationToken);
        if (session is null || !session.IsValid())
        {
            throw new InvalidOperationException("Refresh token is invalid or expired.");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == session.UserId, cancellationToken);
        if (user is null)
        {
            throw new InvalidOperationException("User not found.");
        }

        // Rotate: revoke the used refresh token and issue a fresh pair.
        session.Revoke();

        var roleCodes = await GetRoleCodesAsync(user.UserId, cancellationToken);

        return await IssueTokensAsync(user, roleCodes, session.IpAddress, session.UserAgent, session.DeviceId, cancellationToken);
    }

    public async Task<bool> LogoutAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenHash = HashToken(request.RefreshToken);

        var session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.RefreshTokenHash == tokenHash, cancellationToken);
        if (session is null)
        {
            return false;
        }

        session.Revoke();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task<List<string>> GetRoleCodesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await (
            from userRole in dbContext.UserRoles.AsNoTracking()
            where userRole.UserId == userId
            join role in dbContext.Roles.AsNoTracking() on userRole.RoleId equals role.RoleId
            select role.RoleCode)
            .ToListAsync(cancellationToken);
    }

    private async Task<AuthResponse> IssueTokensAsync(
        User user,
        IReadOnlyCollection<string> roleCodes,
        IPAddress? ipAddress,
        string? userAgent,
        string? deviceId,
        CancellationToken cancellationToken)
    {
        var (accessToken, accessTokenExpiresAt) = jwtTokenService.GenerateAccessToken(user.UserId, user.Email, roleCodes);

        var rawRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTimeOffset.UtcNow.Add(RefreshTokenLifetime);

        var session = UserSession.Create(user.UserId, HashToken(rawRefreshToken), refreshTokenExpiresAt, ipAddress, userAgent, deviceId);
        dbContext.UserSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        var userSummary = new UserSummaryResponse(
            user.UserId,
            user.Email,
            user.MobileNumber,
            user.Status.ToString(),
            user.IsEmailVerified,
            user.IsMobileVerified,
            roleCodes,
            user.CreatedAt);

        return new AuthResponse(accessToken, accessTokenExpiresAt, rawRefreshToken, refreshTokenExpiresAt, userSummary);
    }

    private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private static string HashToken(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
