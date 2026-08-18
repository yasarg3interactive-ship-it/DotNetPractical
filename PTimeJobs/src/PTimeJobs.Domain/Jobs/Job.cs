namespace PTimeJobs.Domain.Jobs;

public sealed class Job
{
    private Job()
    {
    }

    public Guid JobId { get; private set; }
    public Guid EmployerProfileId { get; private set; }
    public Guid? JobCategoryId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public EmploymentType EmploymentType { get; private set; }
    public SalaryModel SalaryModel { get; private set; }
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public int OpeningsCount { get; private set; }
    public int MinExperienceMonths { get; private set; }
    public JobStatus Status { get; private set; }
    public DateTimeOffset? ApplicationDeadline { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static Job Create(
        Guid employerProfileId,
        string title,
        string description,
        EmploymentType employmentType,
        SalaryModel salaryModel,
        decimal? salaryMin = null,
        decimal? salaryMax = null,
        int openingsCount = 1,
        int minExperienceMonths = 0,
        Guid? jobCategoryId = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new InvalidOperationException("Title is required.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new InvalidOperationException("Description is required.");
        }

        if (openingsCount <= 0)
        {
            throw new InvalidOperationException("Openings count must be greater than zero.");
        }

        if (salaryMin.HasValue && salaryMax.HasValue && salaryMin > salaryMax)
        {
            throw new InvalidOperationException("Minimum salary cannot exceed maximum salary.");
        }

        return new Job
        {
            JobId = Guid.NewGuid(),
            EmployerProfileId = employerProfileId,
            JobCategoryId = jobCategoryId,
            Title = title,
            Description = description,
            EmploymentType = employmentType,
            SalaryModel = salaryModel,
            SalaryMin = salaryMin,
            SalaryMax = salaryMax,
            OpeningsCount = openingsCount,
            MinExperienceMonths = minExperienceMonths,
            Status = JobStatus.Draft,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Publish()
    {
        Status = JobStatus.Open;
        PublishedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Pause()
    {
        Status = JobStatus.Paused;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Close()
    {
        Status = JobStatus.Closed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkFilled()
    {
        Status = JobStatus.Filled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        Status = JobStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void SetApplicationDeadline(DateTimeOffset deadline)
    {
        ApplicationDeadline = deadline;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
