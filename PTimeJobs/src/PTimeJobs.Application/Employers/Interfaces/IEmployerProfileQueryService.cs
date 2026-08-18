using PTimeJobs.Application.Employers.Dtos;

namespace PTimeJobs.Application.Employers.Interfaces;

public interface IEmployerProfileQueryService
{
    Task<EmployerProfileResponse?> GetByIdAsync(Guid employerProfileId, CancellationToken cancellationToken = default);

    Task<EmployerProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
