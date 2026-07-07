using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface IUserQueryService
{
    Task<UserSummaryResponse?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
