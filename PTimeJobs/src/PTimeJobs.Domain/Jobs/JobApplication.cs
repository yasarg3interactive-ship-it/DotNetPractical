namespace PTimeJobs.Domain.Jobs;

public sealed class JobApplication
{
    private JobApplication()
    {
    }

    public Guid ApplicationId { get; private set; }
    public Guid JobId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public string? CoverNote { get; private set; }
    public decimal? ExpectedSalary { get; private set; }
    public DateTimeOffset AppliedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static JobApplication Create(
        Guid jobId,
        Guid workerProfileId,
        string? coverNote = null,
        decimal? expectedSalary = null)
    {
        return new JobApplication
        {
            ApplicationId = Guid.NewGuid(),
            JobId = jobId,
            WorkerProfileId = workerProfileId,
            Status = ApplicationStatus.Submitted,
            CoverNote = coverNote,
            ExpectedSalary = expectedSalary,
            AppliedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void ChangeStatus(ApplicationStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
