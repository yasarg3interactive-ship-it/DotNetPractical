using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class JobQueryService(ApplicationDbContext dbContext) : IJobQueryService
{
    public async Task<JobDetailResponse?> GetByIdAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        return await (
            from job in dbContext.Jobs.AsNoTracking()
            where job.JobId == jobId
            join employer in dbContext.EmployerProfiles.AsNoTracking() on job.EmployerProfileId equals employer.EmployerProfileId
            join category in dbContext.JobCategories.AsNoTracking() on job.JobCategoryId equals category.JobCategoryId into categoryJoin
            from category in categoryJoin.DefaultIfEmpty()
            select new JobDetailResponse(
                job.JobId,
                job.EmployerProfileId,
                employer.CompanyName,
                job.JobCategoryId,
                category != null ? category.CategoryName : null,
                job.Title,
                job.Description,
                job.EmploymentType.ToString(),
                job.SalaryModel.ToString(),
                job.SalaryMin,
                job.SalaryMax,
                job.OpeningsCount,
                job.MinExperienceMonths,
                job.Status.ToString(),
                job.ApplicationDeadline,
                job.PublishedAt,
                job.CreatedAt,
                job.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<JobSummaryResponse>> SearchAsync(
        string? status,
        Guid? jobCategoryId,
        string? employmentType,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query =
            from job in dbContext.Jobs.AsNoTracking()
            join employer in dbContext.EmployerProfiles.AsNoTracking() on job.EmployerProfileId equals employer.EmployerProfileId
            join category in dbContext.JobCategories.AsNoTracking() on job.JobCategoryId equals category.JobCategoryId into categoryJoin
            from category in categoryJoin.DefaultIfEmpty()
            select new { job, employer, category };

        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<JobStatus>(status, true, out var statusValue))
        {
            query = query.Where(x => x.job.Status == statusValue);
        }

        if (jobCategoryId.HasValue)
        {
            query = query.Where(x => x.job.JobCategoryId == jobCategoryId);
        }

        if (!string.IsNullOrWhiteSpace(employmentType) && Enum.TryParse<EmploymentType>(employmentType, true, out var employmentTypeValue))
        {
            query = query.Where(x => x.job.EmploymentType == employmentTypeValue);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.job.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new JobSummaryResponse(
                x.job.JobId,
                x.job.EmployerProfileId,
                x.employer.CompanyName,
                x.job.JobCategoryId,
                x.category != null ? x.category.CategoryName : null,
                x.job.Title,
                x.job.EmploymentType.ToString(),
                x.job.SalaryModel.ToString(),
                x.job.SalaryMin,
                x.job.SalaryMax,
                x.job.OpeningsCount,
                x.job.Status.ToString(),
                x.job.PublishedAt,
                x.job.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<JobSummaryResponse>(items, page, pageSize, totalCount);
    }

    public async Task<IReadOnlyCollection<JobLocationResponse>> GetLocationsAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobLocations
            .AsNoTracking()
            .Where(location => location.JobId == jobId)
            .Select(location => new JobLocationResponse(
                location.JobLocationId,
                location.JobId,
                location.LocationId,
                location.Latitude,
                location.Longitude,
                location.IsRemoteAllowed))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobScheduleResponse>> GetSchedulesAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobSchedules
            .AsNoTracking()
            .Where(schedule => schedule.JobId == jobId)
            .Select(schedule => new JobScheduleResponse(
                schedule.JobScheduleId,
                schedule.JobId,
                schedule.DayOfWeek,
                schedule.StartTime,
                schedule.EndTime,
                schedule.StartDate,
                schedule.EndDate,
                schedule.ShiftLabel,
                schedule.RequiredWorkers))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<JobSkillResponse>> GetSkillsAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        return await (
            from jobSkill in dbContext.JobSkills.AsNoTracking()
            where jobSkill.JobId == jobId
            join skill in dbContext.Skills.AsNoTracking() on jobSkill.SkillId equals skill.SkillId
            select new JobSkillResponse(jobSkill.JobId, jobSkill.SkillId, skill.SkillName, jobSkill.RequiredLevel, jobSkill.IsMandatory))
            .ToListAsync(cancellationToken);
    }
}
