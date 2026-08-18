using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reviews.Dtos;

namespace PTimeJobs.Application.Reviews.Interfaces;

public interface IReviewsService
{
    Task<ReviewResponse?> GetByIdAsync(Guid reviewId, CancellationToken cancellationToken = default);

    Task<PagedResult<ReviewResponse>> GetForEntityAsync(
        string targetEntityType,
        Guid targetEntityId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<ReviewResponse> CreateAsync(CreateReviewRequest request, CancellationToken cancellationToken = default);

    Task<ReviewResponse?> FlagAsync(Guid reviewId, CancellationToken cancellationToken = default);

    Task<ReviewResponse?> HideAsync(Guid reviewId, CancellationToken cancellationToken = default);
}
