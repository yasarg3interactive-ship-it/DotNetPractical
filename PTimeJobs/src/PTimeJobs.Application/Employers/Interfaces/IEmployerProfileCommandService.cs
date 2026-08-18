using PTimeJobs.Application.Employers.Dtos;

namespace PTimeJobs.Application.Employers.Interfaces;

public interface IEmployerProfileCommandService
{
    Task<EmployerProfileResponse> CreateAsync(CreateEmployerProfileRequest request, CancellationToken cancellationToken = default);
}
