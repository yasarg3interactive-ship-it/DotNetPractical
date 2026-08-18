namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record CreateJobApplicationRequest(
    Guid JobId,
    Guid WorkerProfileId,
    string? CoverNote,
    decimal? ExpectedSalary);
