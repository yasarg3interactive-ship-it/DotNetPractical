namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record JobDetailResponse(
    Guid JobId,
    Guid EmployerProfileId,
    string CompanyName,
    Guid? JobCategoryId,
    string? CategoryName,
    string Title,
    string Description,
    string EmploymentType,
    string SalaryModel,
    decimal? SalaryMin,
    decimal? SalaryMax,
    int OpeningsCount,
    int MinExperienceMonths,
    string Status,
    DateTimeOffset? ApplicationDeadline,
    DateTimeOffset? PublishedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
