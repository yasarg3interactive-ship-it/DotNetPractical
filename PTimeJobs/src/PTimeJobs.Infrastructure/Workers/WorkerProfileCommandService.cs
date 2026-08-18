using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Workers.Dtos;
using PTimeJobs.Application.Workers.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Workers;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Workers;

public sealed class WorkerProfileCommandService(
    ApplicationDbContext dbContext,
    IWorkerProfileQueryService workerProfileQueryService) : IWorkerProfileCommandService
{
    public async Task<WorkerProfileResponse> CreateAsync(CreateWorkerProfileRequest request, CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users
            .AsNoTracking()
            .AnyAsync(user => user.UserId == request.UserId, cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var alreadyHasProfile = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(profile => profile.UserId == request.UserId, cancellationToken);

        if (alreadyHasProfile)
        {
            throw new InvalidOperationException("This user already has a worker profile.");
        }

        var profile = WorkerProfile.Create(request.UserId);
        dbContext.WorkerProfiles.Add(profile);
        await dbContext.SaveChangesAsync(cancellationToken);

        return (await workerProfileQueryService.GetByIdAsync(profile.WorkerProfileId, cancellationToken))!;
    }

    public async Task<WorkerProfileResponse?> UpdateHeadlineAsync(
        Guid workerProfileId,
        UpdateWorkerHeadlineRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.WorkerProfiles.FirstOrDefaultAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);
        if (profile is null)
        {
            return null;
        }

        profile.UpdateHeadline(request.Headline);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await workerProfileQueryService.GetByIdAsync(workerProfileId, cancellationToken);
    }

    public async Task<WorkerProfileResponse?> UpdateExpectedSalaryAsync(
        Guid workerProfileId,
        UpdateWorkerExpectedSalaryRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await dbContext.WorkerProfiles.FirstOrDefaultAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);
        if (profile is null)
        {
            return null;
        }

        SalaryModel? salaryModel = null;
        if (!string.IsNullOrWhiteSpace(request.SalaryModel))
        {
            if (!Enum.TryParse<SalaryModel>(request.SalaryModel, true, out var parsed))
            {
                throw new InvalidOperationException($"Unknown salary model '{request.SalaryModel}'.");
            }

            salaryModel = parsed;
        }

        profile.UpdateExpectedSalary(request.Min, request.Max, salaryModel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await workerProfileQueryService.GetByIdAsync(workerProfileId, cancellationToken);
    }

    public async Task<WorkerProfileResponse?> AddSkillAsync(
        Guid workerProfileId,
        AddWorkerSkillRequest request,
        CancellationToken cancellationToken = default)
    {
        var profileExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);

        if (!profileExists)
        {
            return null;
        }

        var skillExists = await dbContext.Skills.AsNoTracking().AnyAsync(skill => skill.SkillId == request.SkillId, cancellationToken);
        if (!skillExists)
        {
            throw new InvalidOperationException("Skill not found.");
        }

        var alreadyAdded = await dbContext.WorkerSkills
            .AsNoTracking()
            .AnyAsync(
                ws => ws.WorkerProfileId == workerProfileId && ws.SkillId == request.SkillId,
                cancellationToken);

        if (alreadyAdded)
        {
            throw new InvalidOperationException("This skill has already been added to the profile.");
        }

        var workerSkill = WorkerSkill.Create(
            workerProfileId,
            request.SkillId,
            request.ProficiencyLevel,
            request.YearsExperience,
            request.IsPrimary);

        dbContext.WorkerSkills.Add(workerSkill);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await workerProfileQueryService.GetByIdAsync(workerProfileId, cancellationToken);
    }

    public async Task<WorkerProfileResponse?> AddExperienceAsync(
        Guid workerProfileId,
        AddWorkerExperienceRequest request,
        CancellationToken cancellationToken = default)
    {
        var profileExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);

        if (!profileExists)
        {
            return null;
        }

        EmploymentType? employmentType = null;
        if (!string.IsNullOrWhiteSpace(request.EmploymentType))
        {
            if (!Enum.TryParse<EmploymentType>(request.EmploymentType, true, out var parsed))
            {
                throw new InvalidOperationException($"Unknown employment type '{request.EmploymentType}'.");
            }

            employmentType = parsed;
        }

        var experience = WorkerExperience.Create(
            workerProfileId,
            request.JobTitle,
            request.CompanyName,
            employmentType,
            request.StartDate,
            request.EndDate,
            request.Description);

        dbContext.WorkerExperiences.Add(experience);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await workerProfileQueryService.GetByIdAsync(workerProfileId, cancellationToken);
    }

    public async Task<WorkerProfileResponse?> AddEducationAsync(
        Guid workerProfileId,
        AddWorkerEducationRequest request,
        CancellationToken cancellationToken = default)
    {
        var profileExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(p => p.WorkerProfileId == workerProfileId, cancellationToken);

        if (!profileExists)
        {
            return null;
        }

        var education = WorkerEducation.Create(
            workerProfileId,
            request.InstitutionName,
            request.Degree,
            request.FieldOfStudy,
            request.StartYear,
            request.EndYear,
            request.IsCurrent);

        dbContext.WorkerEducations.Add(education);
        await dbContext.SaveChangesAsync(cancellationToken);

        return await workerProfileQueryService.GetByIdAsync(workerProfileId, cancellationToken);
    }
}
