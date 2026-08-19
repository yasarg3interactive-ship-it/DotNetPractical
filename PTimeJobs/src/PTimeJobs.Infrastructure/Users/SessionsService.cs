using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class SessionsService(ApplicationDbContext dbContext) : ISessionsService
{
    public async Task<IReadOnlyCollection<UserSessionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.UserSessions
            .AsNoTracking()
            .Where(session => session.UserId == userId)
            .OrderByDescending(session => session.CreatedAt)
            .Select(session => new UserSessionResponse(
                session.SessionId,
                session.UserId,
                session.Status.ToString(),
                session.UserAgent,
                session.DeviceId,
                session.CreatedAt,
                session.ExpiresAt,
                session.RevokedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> RevokeAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await dbContext.UserSessions.FirstOrDefaultAsync(s => s.SessionId == sessionId, cancellationToken);
        if (session is null)
        {
            return false;
        }

        session.Revoke();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
