using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface ISessionsService
{
    Task<IReadOnlyCollection<UserSessionResponse>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> RevokeAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
