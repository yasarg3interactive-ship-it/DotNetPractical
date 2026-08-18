using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class SkillsService(ApplicationDbContext dbContext) : ISkillsService
{
    public async Task<IReadOnlyCollection<SkillResponse>> GetAllAsync(string? search, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Skills.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalized = search.Trim().ToLowerInvariant();
            query = query.Where(skill => skill.NormalizedName.Contains(normalized));
        }

        return await query
            .OrderBy(skill => skill.SkillName)
            .Select(skill => new SkillResponse(skill.SkillId, skill.SkillName, skill.SkillCategory, skill.IsVerified))
            .ToListAsync(cancellationToken);
    }

    public async Task<SkillResponse?> GetByIdAsync(Guid skillId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Skills
            .AsNoTracking()
            .Where(skill => skill.SkillId == skillId)
            .Select(skill => new SkillResponse(skill.SkillId, skill.SkillName, skill.SkillCategory, skill.IsVerified))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<SkillResponse> CreateAsync(CreateSkillRequest request, CancellationToken cancellationToken = default)
    {
        var normalized = request.SkillName.Trim().ToLowerInvariant();

        var duplicate = await dbContext.Skills
            .AsNoTracking()
            .AnyAsync(skill => skill.NormalizedName == normalized, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("A skill with this name already exists.");
        }

        var skill = Skill.Create(request.SkillName, request.SkillCategory);
        dbContext.Skills.Add(skill);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new SkillResponse(skill.SkillId, skill.SkillName, skill.SkillCategory, skill.IsVerified);
    }
}
