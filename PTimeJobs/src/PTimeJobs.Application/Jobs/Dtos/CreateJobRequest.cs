namespace PTimeJobs.Application.Jobs.Dtos;

/// <summary>
/// EmploymentType / SalaryModel are the enum names as strings (e.g. "FullTime", "Monthly") —
/// see PTimeJobs.Domain.Jobs.EmploymentType / SalaryModel for the full list of values.
/// </summary>
public sealed record CreateJobRequest(
    Guid EmployerProfileId,
    string Title,
    string Description,
    string EmploymentType,
    string SalaryModel,
    decimal? SalaryMin,
    decimal? SalaryMax,
    int OpeningsCount,
    int MinExperienceMonths,
    Guid? JobCategoryId);
