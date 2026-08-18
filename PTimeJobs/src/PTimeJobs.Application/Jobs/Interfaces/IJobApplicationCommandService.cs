using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IJobApplicationCommandService
{
    Task<JobApplicationResponse> CreateAsync(CreateJobApplicationRequest request, CancellationToken cancellationToken = default);

    Task<JobApplicationResponse?> UpdateStatusAsync(Guid applicationId, string status, CancellationToken cancellationToken = default);
}
