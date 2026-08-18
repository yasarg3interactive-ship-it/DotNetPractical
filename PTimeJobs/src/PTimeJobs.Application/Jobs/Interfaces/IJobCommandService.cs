using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IJobCommandService
{
    Task<JobDetailResponse> CreateAsync(CreateJobRequest request, CancellationToken cancellationToken = default);

    Task<JobDetailResponse?> PublishAsync(Guid jobId, CancellationToken cancellationToken = default);

    Task<JobDetailResponse?> CloseAsync(Guid jobId, CancellationToken cancellationToken = default);
}
