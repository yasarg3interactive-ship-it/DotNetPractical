using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IJobApplicationQueryService
{
    Task<JobApplicationResponse?> GetByIdAsync(Guid applicationId, CancellationToken cancellationToken = default);

    Task<PagedResult<JobApplicationResponse>> SearchAsync(
        Guid? jobId,
        Guid? workerProfileId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
