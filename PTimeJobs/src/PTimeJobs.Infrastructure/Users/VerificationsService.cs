using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class VerificationsService(ApplicationDbContext dbContext) : IVerificationsService
{
    public async Task<VerificationResponse?> GetByIdAsync(Guid verificationId, CancellationToken cancellationToken = default)
    {
        var verification = await dbContext.Verifications
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.VerificationId == verificationId, cancellationToken);

        return verification is null ? null : ToResponse(verification);
    }

    public async Task<IReadOnlyCollection<VerificationResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var verifications = await dbContext.Verifications
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.RequestedAt)
            .ToListAsync(cancellationToken);

        return verifications.Select(ToResponse).ToList();
    }

    public async Task<VerificationResponse> CreateAsync(CreateVerificationRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == request.UserId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        if (!Enum.TryParse<VerificationChannel>(request.Channel, true, out var channel))
        {
            throw new InvalidOperationException($"Unknown verification channel '{request.Channel}'.");
        }

        var verification = Verification.Create(request.UserId, channel, request.TargetValue, request.TokenHash, request.ExpiresAt);
        dbContext.Verifications.Add(verification);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(verification);
    }

    public async Task<VerificationResponse?> VerifyAsync(Guid verificationId, CancellationToken cancellationToken = default)
    {
        var verification = await dbContext.Verifications.FirstOrDefaultAsync(v => v.VerificationId == verificationId, cancellationToken);
        if (verification is null)
        {
            return null;
        }

        verification.MarkVerified();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(verification);
    }

    public async Task<VerificationResponse?> MarkFailedAsync(Guid verificationId, CancellationToken cancellationToken = default)
    {
        var verification = await dbContext.Verifications.FirstOrDefaultAsync(v => v.VerificationId == verificationId, cancellationToken);
        if (verification is null)
        {
            return null;
        }

        verification.MarkFailed();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(verification);
    }

    public async Task<VerificationResponse?> RevokeAsync(Guid verificationId, CancellationToken cancellationToken = default)
    {
        var verification = await dbContext.Verifications.FirstOrDefaultAsync(v => v.VerificationId == verificationId, cancellationToken);
        if (verification is null)
        {
            return null;
        }

        verification.Revoke();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(verification);
    }

    private static VerificationResponse ToResponse(Verification verification) => new(
        verification.VerificationId,
        verification.UserId,
        verification.Channel.ToString(),
        verification.TargetValue,
        verification.Status.ToString(),
        verification.RequestedAt,
        verification.VerifiedAt,
        verification.ExpiresAt);
}
