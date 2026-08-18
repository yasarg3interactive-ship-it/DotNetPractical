using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Domain.Workers;

public sealed class WorkerExperience
{
    private WorkerExperience()
    {
    }

    public Guid ExperienceId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public string? CompanyName { get; private set; }
    public string JobTitle { get; private set; } = string.Empty;
    public EmploymentType? EmploymentType { get; private set; }
    public DateOnly? StartDate { get; private set; }
    public DateOnly? EndDate { get; private set; }
    public string? Description { get; private set; }
    public Guid? LocationId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static WorkerExperience Create(
        Guid workerProfileId,
        string jobTitle,
        string? companyName = null,
        EmploymentType? employmentType = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        string? description = null,
        Guid? locationId = null)
    {
        if (string.IsNullOrWhiteSpace(jobTitle))
        {
            throw new InvalidOperationException("Job title is required.");
        }

        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            throw new InvalidOperationException("Start date cannot be after end date.");
        }

        return new WorkerExperience
        {
            ExperienceId = Guid.NewGuid(),
            WorkerProfileId = workerProfileId,
            CompanyName = companyName,
            JobTitle = jobTitle,
            EmploymentType = employmentType,
            StartDate = startDate,
            EndDate = endDate,
            Description = description,
            LocationId = locationId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
