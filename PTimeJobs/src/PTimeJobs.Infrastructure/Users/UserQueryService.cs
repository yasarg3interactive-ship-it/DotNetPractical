using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class UserQueryService(ApplicationDbContext dbContext) : IUserQueryService
{
    public async Task<UserSummaryResponse?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .Where(user => user.UserId == userId)
            .Select(user => new UserSummaryResponse(
                user.UserId,
                user.Email,
                user.MobileNumber,
                user.Status.ToString(),
                user.IsEmailVerified,
                user.IsMobileVerified,
                user.UserRoles
                    .Select(userRole => userRole.Role.RoleCode)
                    .OrderBy(roleCode => roleCode)
                    .ToArray(),
                user.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
