using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class JobApplicationCommandService(
    ApplicationDbContext dbContext,
    IJobApplicationQueryService jobApplicationQueryService) : IJobApplicationCommandService
{
    public async Task<JobApplicationResponse> CreateAsync(CreateJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var job = await dbContext.Jobs
            .AsNoTracking()
            .FirstOrDefaultAsync(j => j.JobId == request.JobId, cancellationToken);

        if (job is null)
        {
            throw new InvalidOperationException("Job not found.");
        }

        if (job.Status != JobStatus.Open)
        {
            throw new InvalidOperationException("This job is not open for applications.");
        }

        var workerExists = await dbContext.WorkerProfiles
            .AsNoTracking()
            .AnyAsync(worker => worker.WorkerProfileId == request.WorkerProfileId, cancellationToken);

        if (!workerExists)
        {
            throw new InvalidOperationException("Worker profile not found.");
        }

        var alreadyApplied = await dbContext.JobApplications
            .AsNoTracking()
            .AnyAsync(
                application => application.JobId == request.JobId && application.WorkerProfileId == request.WorkerProfileId,
                cancellationToken);

        if (alreadyApplied)
        {
            throw new InvalidOperationException("You have already applied to this job.");
        }

        var jobApplication = JobApplication.Create(
            request.JobId,
            request.WorkerProfileId,
            request.CoverNote,
            request.ExpectedSalary);

        dbContext.JobApplications.Add(jobApplication);
        await dbContext.SaveChangesAsync(cancellationToken);

        return (await jobApplicationQueryService.GetByIdAsync(jobApplication.ApplicationId, cancellationToken))!;
    }

    public async Task<JobApplicationResponse?> UpdateStatusAsync(Guid applicationId, string status, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ApplicationStatus>(status, true, out var statusValue))
        {
            throw new InvalidOperationException($"Unknown application status '{status}'.");
        }

        var application = await dbContext.JobApplications
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId, cancellationToken);

        if (application is null)
        {
            return null;
        }

        var oldStatus = application.Status;
        application.ChangeStatus(statusValue);

        // Record the transition for audit/history purposes.
        var historyEntry = HiringStatusHistory.Create(applicationId, oldStatus, statusValue);
        dbContext.HiringStatusHistories.Add(historyEntry);

        await dbContext.SaveChangesAsync(cancellationToken);

        return await jobApplicationQueryService.GetByIdAsync(applicationId, cancellationToken);
    }

    public async Task<ShortlistResponse?> AddShortlistAsync(Guid applicationId, AddShortlistRequest request, CancellationToken cancellationToken = default)
    {
        var applicationExists = await dbContext.JobApplications
            .AsNoTracking()
            .AnyAsync(a => a.ApplicationId == applicationId, cancellationToken);

        if (!applicationExists)
        {
            return null;
        }

        var shortlist = Shortlist.Create(applicationId, request.ShortlistedBy, request.Notes);
        dbContext.Shortlists.Add(shortlist);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ShortlistResponse(shortlist.ShortlistId, shortlist.ApplicationId, shortlist.ShortlistedBy, shortlist.Notes, shortlist.CreatedAt);
    }
}
