namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record JobApplicationResponse(
    Guid ApplicationId,
    Guid JobId,
    string JobTitle,
    Guid WorkerProfileId,
    string Status,
    string? CoverNote,
    decimal? ExpectedSalary,
    DateTimeOffset AppliedAt,
    DateTimeOffset UpdatedAt);
