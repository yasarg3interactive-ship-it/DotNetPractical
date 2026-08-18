using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Employers.Dtos;
using PTimeJobs.Application.Employers.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Employers;

public sealed class EmployerProfileQueryService(ApplicationDbContext dbContext) : IEmployerProfileQueryService
{
    public async Task<EmployerProfileResponse?> GetByIdAsync(Guid employerProfileId, CancellationToken cancellationToken = default)
    {
        return await dbContext.EmployerProfiles
            .AsNoTracking()
            .Where(profile => profile.EmployerProfileId == employerProfileId)
            .Select(profile => new EmployerProfileResponse(
                profile.EmployerProfileId,
                profile.UserId,
                profile.CompanyName,
                profile.BusinessType,
                profile.RegistrationNumber,
                profile.VerificationStatus.ToString(),
                profile.LocationId,
                profile.AverageRating,
                profile.RatingCount,
                profile.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<EmployerProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.EmployerProfiles
            .AsNoTracking()
            .Where(profile => profile.UserId == userId)
            .Select(profile => new EmployerProfileResponse(
                profile.EmployerProfileId,
                profile.UserId,
                profile.CompanyName,
                profile.BusinessType,
                profile.RegistrationNumber,
                profile.VerificationStatus.ToString(),
                profile.LocationId,
                profile.AverageRating,
                profile.RatingCount,
                profile.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
