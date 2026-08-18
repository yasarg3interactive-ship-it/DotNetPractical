using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface IMatchingScoresService
{
    Task<IReadOnlyCollection<MatchingScoreResponse>> GetByJobAsync(Guid jobId, int top, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MatchingScoreResponse>> GetByWorkerAsync(Guid workerProfileId, int top, CancellationToken cancellationToken = default);

    Task<MatchingScoreResponse> CreateAsync(CreateMatchingScoreRequest request, CancellationToken cancellationToken = default);
}
