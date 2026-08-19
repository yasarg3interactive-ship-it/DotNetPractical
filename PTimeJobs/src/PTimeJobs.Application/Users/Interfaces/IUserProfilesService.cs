using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface IUserProfilesService
{
    Task<UserProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserProfileResponse> UpsertAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default);
}
