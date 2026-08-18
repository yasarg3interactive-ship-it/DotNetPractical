using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Employers.Dtos;
using PTimeJobs.Application.Employers.Interfaces;
using PTimeJobs.Domain.Employers;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Employers;

public sealed class EmployerProfileCommandService(
    ApplicationDbContext dbContext,
    IEmployerProfileQueryService employerProfileQueryService) : IEmployerProfileCommandService
{
    public async Task<EmployerProfileResponse> CreateAsync(CreateEmployerProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.UserId == request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var alreadyHasProfile = await dbContext.EmployerProfiles
            .AsNoTracking()
            .AnyAsync(profile => profile.UserId == request.UserId, cancellationToken);

        if (alreadyHasProfile)
        {
            throw new InvalidOperationException("This user already has an employer profile.");
        }

        if (request.LocationId.HasValue)
        {
            var locationExists = await dbContext.Locations
                .AsNoTracking()
                .AnyAsync(location => location.LocationId == request.LocationId, cancellationToken);

            if (!locationExists)
            {
                throw new InvalidOperationException("Location not found.");
            }
        }

        var profile = EmployerProfile.Create(
            request.UserId,
            request.CompanyName,
            request.BusinessType,
            request.RegistrationNumber,
            request.LocationId);

        dbContext.EmployerProfiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return (await employerProfileQueryService.GetByIdAsync(profile.EmployerProfileId, cancellationToken))!;
    }
}
