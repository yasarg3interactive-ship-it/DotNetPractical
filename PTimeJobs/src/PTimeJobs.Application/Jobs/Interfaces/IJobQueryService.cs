using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IJobQueryService
{
    Task<JobDetailResponse?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default);

    Task<PagedResult<JobSummaryResponse>> SearchAsync(
        string? status,
        Guid? jobCategoryId,
        string? employmentType,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
