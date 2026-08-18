using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class JobCommandService(ApplicationDbContext dbContext, IJobQueryService jobQueryService) : IJobCommandService
{
    public async Task<JobDetailResponse> CreateAsync(CreateJobRequest request, CancellationToken cancellationToken = default)
    {
        var employerExists = await dbContext.EmployerProfiles
            .AsNoTracking()
            .AnyAsync(employer => employer.EmployerProfileId == request.EmployerProfileId, cancellationToken);

        if (!employerExists)
        {
            throw new InvalidOperationException("Employer profile not found.");
        }

        if (!Enum.TryParse<EmploymentType>(request.EmploymentType, true, out var employmentType))
        {
            throw new InvalidOperationException($"Unknown employment type '{request.EmploymentType}'.");
        }

        if (!Enum.TryParse<SalaryModel>(request.SalaryModel, true, out var salaryModel))
        {
            throw new InvalidOperationException($"Unknown salary model '{request.SalaryModel}'.");
        }

        if (request.JobCategoryId.HasValue)
        {
            var categoryExists = await dbContext.JobCategories
                .AsNoTracking()
                .AnyAsync(category => category.JobCategoryId == request.JobCategoryId, cancellationToken);

            if (!categoryExists)
            {
                throw new InvalidOperationException("Job category not found.");
            }
        }

        var job = Job.Create(
            request.EmployerProfileId,
            request.Title,
            request.Description,
            employmentType,
            salaryModel,
            request.SalaryMin,
            request.SalaryMax,
            request.OpeningsCount,
            request.MinExperienceMonths,
            request.JobCategoryId);

        dbContext.Jobs.Add(job);
        await dbContext.SaveChangesAsync(cancellationToken);

        return (await jobQueryService.GetByIdAsync(job.JobId, cancellationToken))!;
    }

    public async Task<JobDetailResponse?> PublishAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId, cancellationToken);
        if (job is null)
        {
            return null;
        }

        job.Publish();
        await dbContext.SaveChangesAsync(cancellationToken);

        return await jobQueryService.GetByIdAsync(jobId, cancellationToken);
    }

    public async Task<JobDetailResponse?> CloseAsync(Guid jobId, CancellationToken cancellationToken = default)
    {
        var job = await dbContext.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId, cancellationToken);
        if (job is null)
        {
            return null;
        }

        job.Close();
        await dbContext.SaveChangesAsync(cancellationToken);

        return await jobQueryService.GetByIdAsync(jobId, cancellationToken);
    }
}
