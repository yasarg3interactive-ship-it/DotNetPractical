using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface IVerificationsService
{
    Task<VerificationResponse?> GetByIdAsync(Guid verificationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<VerificationResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<VerificationResponse> CreateAsync(CreateVerificationRequest request, CancellationToken cancellationToken = default);

    Task<VerificationResponse?> VerifyAsync(Guid verificationId, CancellationToken cancellationToken = default);

    Task<VerificationResponse?> MarkFailedAsync(Guid verificationId, CancellationToken cancellationToken = default);

    Task<VerificationResponse?> RevokeAsync(Guid verificationId, CancellationToken cancellationToken = default);
}
