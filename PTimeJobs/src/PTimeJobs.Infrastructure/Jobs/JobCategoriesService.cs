using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Jobs.Dtos;
using PTimeJobs.Application.Jobs.Interfaces;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Jobs;

public sealed class JobCategoriesService(ApplicationDbContext dbContext) : IJobCategoriesService
{
    public async Task<IReadOnlyCollection<JobCategoryResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.JobCategories
            .AsNoTracking()
            .OrderBy(category => category.CategoryName)
            .Select(category => new JobCategoryResponse(
                category.JobCategoryId,
                category.ParentCategoryId,
                category.CategoryName,
                category.CategorySlug,
                category.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<JobCategoryResponse?> GetByIdAsync(Guid jobCategoryId, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobCategories
            .AsNoTracking()
            .Where(category => category.JobCategoryId == jobCategoryId)
            .Select(category => new JobCategoryResponse(
                category.JobCategoryId,
                category.ParentCategoryId,
                category.CategoryName,
                category.CategorySlug,
                category.IsActive))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<JobCategoryResponse> CreateAsync(CreateJobCategoryRequest request, CancellationToken cancellationToken = default)
    {
        var slugTaken = await dbContext.JobCategories
            .AsNoTracking()
            .AnyAsync(category => category.CategorySlug == request.CategorySlug, cancellationToken);

        if (slugTaken)
        {
            throw new InvalidOperationException("A category with this slug already exists.");
        }

        if (request.ParentCategoryId.HasValue)
        {
            var parentExists = await dbContext.JobCategories
                .AsNoTracking()
                .AnyAsync(category => category.JobCategoryId == request.ParentCategoryId, cancellationToken);

            if (!parentExists)
            {
                throw new InvalidOperationException("Parent category not found.");
            }
        }

        var category = JobCategory.Create(request.CategoryName, request.CategorySlug, request.ParentCategoryId);
        dbContext.JobCategories.Add(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new JobCategoryResponse(category.JobCategoryId, category.ParentCategoryId, category.CategoryName, category.CategorySlug, category.IsActive);
    }
}
