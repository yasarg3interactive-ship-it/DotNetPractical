namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record JobCategoryResponse(Guid JobCategoryId, Guid? ParentCategoryId, string CategoryName, string CategorySlug, bool IsActive);

public sealed record CreateJobCategoryRequest(string CategoryName, string CategorySlug, Guid? ParentCategoryId);
