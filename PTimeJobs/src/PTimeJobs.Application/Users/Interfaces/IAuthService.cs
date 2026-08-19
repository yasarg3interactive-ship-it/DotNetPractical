using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);

    Task<AuthResponse> LoginAsync(LoginRequest request, string? ipAddress, string? userAgent, CancellationToken cancellationToken = default);

    Task<AuthResponse> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

    Task<bool> LogoutAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
}
