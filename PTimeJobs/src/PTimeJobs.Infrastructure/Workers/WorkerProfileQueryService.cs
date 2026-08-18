using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Workers.Dtos;
using PTimeJobs.Application.Workers.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Workers;

public sealed class WorkerProfileQueryService(ApplicationDbContext dbContext) : IWorkerProfileQueryService
{
    public async Task<WorkerProfileResponse?> GetByIdAsync(Guid workerProfileId, CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.WorkerProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);

        if (profile is null)
        {
            return null;
        }

        var skills = await (
            from workerSkill in dbContext.WorkerSkills.AsNoTracking()
            where workerSkill.WorkerProfileId == workerProfileId
            join skill in dbContext.Skills.AsNoTracking() on workerSkill.SkillId equals skill.SkillId
            select new WorkerSkillResponse(
                skill.SkillId,
                skill.SkillName,
                workerSkill.ProficiencyLevel,
                workerSkill.YearsExperience,
                workerSkill.IsPrimary,
                workerSkill.VerifiedAt))
            .ToListAsync(cancellationToken);

        var experience = await dbContext.WorkerExperiences
            .AsNoTracking()
            .Where(exp => exp.WorkerProfileId == workerProfileId)
            .OrderByDescending(exp => exp.StartDate)
            .Select(exp => new WorkerExperienceResponse(
                exp.ExperienceId,
                exp.CompanyName,
                exp.JobTitle,
                exp.EmploymentType != null ? exp.EmploymentType.ToString() : null,
                exp.StartDate,
                exp.EndDate,
                exp.Description))
            .ToListAsync(cancellationToken);

        var education = await dbContext.WorkerEducations
            .AsNoTracking()
            .Where(edu => edu.WorkerProfileId == workerProfileId)
            .OrderByDescending(edu => edu.StartYear)
            .Select(edu => new WorkerEducationResponse(
                edu.EducationId,
                edu.InstitutionName,
                edu.Degree,
                edu.FieldOfStudy,
                edu.StartYear,
                edu.EndYear,
                edu.IsCurrent))
            .ToListAsync(cancellationToken);

        return new WorkerProfileResponse(
            profile.WorkerProfileId,
            profile.UserId,
            profile.Headline,
            profile.ExpectedSalaryMin,
            profile.ExpectedSalaryMax,
            profile.ExpectedSalaryModel?.ToString(),
            profile.TotalExperienceMonths,
            profile.ResumeUrl,
            profile.AverageRating,
            profile.RatingCount,
            skills,
            experience,
            education);
    }

    public async Task<WorkerProfileResponse?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var workerProfileId = await dbContext.WorkerProfiles
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.WorkerProfileId)
            .FirstOrDefaultAsync(cancellationToken);

        return workerProfileId == Guid.Empty ? null : await GetByIdAsync(workerProfileId, cancellationToken);
    }
}
