using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class UserProfilesService(ApplicationDbContext dbContext) : IUserProfilesService
{
    public async Task<UserProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.UserProfiles.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        return profile is null ? null : ToResponse(profile);
    }

    public async Task<UserProfileResponse> UpsertAsync(Guid userId, UpdateUserProfileRequest request, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == userId, cancellationToken);
            if (!userExists)
            {
                throw new InvalidOperationException("User not found.");
            }

            profile = UserProfile.Create(userId);
            dbContext.UserProfiles.Add(profile);
        }

        profile.UpdateDetails(request.FirstName, request.LastName, request.DisplayName, request.DateOfBirth, request.Gender, request.Bio);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(profile);
    }

    private static UserProfileResponse ToResponse(UserProfile profile) => new(
        profile.UserId,
        profile.FirstName,
        profile.LastName,
        profile.DisplayName,
        profile.DateOfBirth,
        profile.Gender,
        profile.ProfilePhotoUrl,
        profile.Bio,
        profile.DefaultLocationId,
        profile.PreferredLanguage,
        profile.Timezone,
        profile.UpdatedAt);
}
