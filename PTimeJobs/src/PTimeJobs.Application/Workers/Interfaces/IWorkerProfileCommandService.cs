using PTimeJobs.Application.Workers.Dtos;

namespace PTimeJobs.Application.Workers.Interfaces;

public interface IWorkerProfileCommandService
{
    Task<WorkerProfileResponse> CreateAsync(CreateWorkerProfileRequest request, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> UpdateHeadlineAsync(Guid workerProfileId, UpdateWorkerHeadlineRequest request, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> UpdateExpectedSalaryAsync(Guid workerProfileId, UpdateWorkerExpectedSalaryRequest request, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> AddSkillAsync(Guid workerProfileId, AddWorkerSkillRequest request, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> AddExperienceAsync(Guid workerProfileId, AddWorkerExperienceRequest request, CancellationToken cancellationToken = default);

    Task<WorkerProfileResponse?> AddEducationAsync(Guid workerProfileId, AddWorkerEducationRequest request, CancellationToken cancellationToken = default);
}
