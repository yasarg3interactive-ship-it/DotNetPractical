using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IJobCategoriesService
{
    Task<IReadOnlyCollection<JobCategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<JobCategoryResponse?> GetByIdAsync(Guid jobCategoryId, CancellationToken cancellationToken = default);

    Task<JobCategoryResponse> CreateAsync(CreateJobCategoryRequest request, CancellationToken cancellationToken = default);
}
