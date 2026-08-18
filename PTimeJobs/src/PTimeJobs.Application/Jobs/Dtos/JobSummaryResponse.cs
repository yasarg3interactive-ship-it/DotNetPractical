namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record JobSummaryResponse(
    Guid JobId,
    Guid EmployerProfileId,
    string CompanyName,
    Guid? JobCategoryId,
    string? CategoryName,
    string Title,
    string EmploymentType,
    string SalaryModel,
    decimal? SalaryMin,
    decimal? SalaryMax,
    int OpeningsCount,
    string Status,
    DateTimeOffset? PublishedAt,
    DateTimeOffset CreatedAt);
