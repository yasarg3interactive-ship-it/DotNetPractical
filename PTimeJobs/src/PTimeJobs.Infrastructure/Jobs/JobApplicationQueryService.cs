using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class JobApplicationQueryService(ApplicationDbContext dbContext) : IJobApplicationQueryService
{
    public async Task<JobApplicationResponse?> GetByIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        return await (
            from application in dbContext.JobApplications.AsNoTracking()
            where application.ApplicationId == applicationId
            join job in dbContext.Jobs.AsNoTracking() on application.JobId equals job.JobId
            select new JobApplicationResponse(
                application.ApplicationId,
                application.JobId,
                job.Title,
                application.WorkerProfileId,
                application.Status.ToString(),
                application.CoverNote,
                application.ExpectedSalary,
                application.AppliedAt,
                application.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedResult<JobApplicationResponse>> SearchAsync(
        Guid? jobId,
        Guid? workerProfileId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query =
            from application in dbContext.JobApplications.AsNoTracking()
            join job in dbContext.Jobs.AsNoTracking() on application.JobId equals job.JobId
            select new { application, job };

        if (jobId.HasValue)
        {
            query = query.Where(x => x.application.JobId == jobId);
        }

        if (workerProfileId.HasValue)
        {
            query = query.Where(x => x.application.WorkerProfileId == workerProfileId);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.application.AppliedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new JobApplicationResponse(
                x.application.ApplicationId,
                x.application.JobId,
                x.job.Title,
                x.application.WorkerProfileId,
                x.application.Status.ToString(),
                x.application.CoverNote,
                x.application.ExpectedSalary,
                x.application.AppliedAt,
                x.application.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<JobApplicationResponse>(items, page, pageSize, totalCount);
    }
}
