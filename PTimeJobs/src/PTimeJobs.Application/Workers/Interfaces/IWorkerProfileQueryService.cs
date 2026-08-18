using PTimeJobs.Application.Workers.Dtos;

namespace PTimeJobs.Application.Workers.Interfaces;

public interface IWorkerProfileQueryService
{
    Task<WorkerProfileResponse?> GetByIdAsync(Guid workerProfileId, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
