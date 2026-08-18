namespace PTimeJobs.Application.Workers.Dtos;

public sealed record WorkerSkillResponse(
    Guid SkillId,
    string SkillName,
    short ProficiencyLevel,
    decimal? YearsExperience,
    bool IsPrimary,
    DateTimeOffset? VerifiedAt);

public sealed record WorkerExperienceResponse(
    Guid ExperienceId,
    string? CompanyName,
    string JobTitle,
    string? EmploymentType,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string? Description);

public sealed record WorkerEducationResponse(
    Guid EducationId,
    string InstitutionName,
    string? Degree,
    string? FieldOfStudy,
    short? StartYear,
    short? EndYear,
    bool IsCurrent);

public sealed record WorkerProfileResponse(
    Guid WorkerProfileId,
    Guid UserId,
    string? Headline,
    decimal? ExpectedSalaryMin,
    decimal? ExpectedSalaryMax,
    string? ExpectedSalaryModel,
    int TotalExperienceMonths,
    string? ResumeUrl,
    decimal AverageRating,
    int RatingCount,
    IReadOnlyCollection<WorkerSkillResponse> Skills,
    IReadOnlyCollection<WorkerExperienceResponse> Experience,
    IReadOnlyCollection<WorkerEducationResponse> Education);

public sealed record CreateWorkerProfileRequest(Guid UserId);

public sealed record UpdateWorkerHeadlineRequest(string? Headline);

public sealed record UpdateWorkerExpectedSalaryRequest(decimal? Min, decimal? Max, string? SalaryModel);

public sealed record AddWorkerSkillRequest(Guid SkillId, short ProficiencyLevel, decimal? YearsExperience, bool IsPrimary);

public sealed record AddWorkerExperienceRequest(
    string JobTitle,
    string? CompanyName,
    string? EmploymentType,
    DateOnly? StartDate,
    DateOnly? EndDate,
    string? Description);

public sealed record AddWorkerEducationRequest(
    string InstitutionName,
    string? Degree,
    string? FieldOfStudy,
    short? StartYear,
    short? EndYear,
    bool IsCurrent);
