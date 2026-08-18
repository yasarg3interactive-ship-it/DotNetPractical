namespace PTimeJobs.Application.Jobs.Dtos;

/// <summary>Status is the enum name as a string — see PTimeJobs.Domain.Jobs.ApplicationStatus.</summary>
public sealed record UpdateJobApplicationStatusRequest(string Status);
