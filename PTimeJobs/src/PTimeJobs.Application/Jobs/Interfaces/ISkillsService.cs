using PTimeJobs.Application.Jobs.Dtos;

namespace PTimeJobs.Application.Jobs.Interfaces;

public interface ISkillsService
{
    Task<IReadOnlyCollection<SkillResponse>> GetAllAsync(string? search, CancellationToken cancellationToken = default);

    Task<SkillResponse?> GetByIdAsync(Guid skillId, CancellationToken cancellationToken = default);

    Task<SkillResponse> CreateAsync(CreateSkillRequest request, CancellationToken cancellationToken = default);
}
