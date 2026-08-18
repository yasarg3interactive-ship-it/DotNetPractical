using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Domain.Workers;

public sealed class WorkerProfile
{
    private readonly List<WorkerSkill> _skills = [];

    private WorkerProfile()
    {
    }

    public Guid WorkerProfileId { get; private set; }
    public Guid UserId { get; private set; }
    public string? Headline { get; private set; }
    public decimal? ExpectedSalaryMin { get; private set; }
    public decimal? ExpectedSalaryMax { get; private set; }
    public SalaryModel? ExpectedSalaryModel { get; private set; }
    public int TotalExperienceMonths { get; private set; }
    public Guid? CurrentLocationId { get; private set; }
    public string? ResumeUrl { get; private set; }
    public decimal? ProfileStrengthScore { get; private set; }
    public decimal AverageRating { get; private set; }
    public int RatingCount { get; private set; }
    public string MatchingMetadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public IReadOnlyCollection<WorkerSkill> Skills => _skills.AsReadOnly();

    public static WorkerProfile Create(Guid userId)
    {
        return new WorkerProfile
        {
            WorkerProfileId = Guid.NewGuid(),
            UserId = userId,
            TotalExperienceMonths = 0,
            AverageRating = 0m,
            RatingCount = 0,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    public void UpdateHeadline(string? headline)
    {
        Headline = headline;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateExpectedSalary(decimal? min, decimal? max, SalaryModel? model)
    {
        if (min.HasValue && max.HasValue && min > max)
        {
            throw new InvalidOperationException("Minimum expected salary cannot exceed maximum.");
        }

        ExpectedSalaryMin = min;
        ExpectedSalaryMax = max;
        ExpectedSalaryModel = model;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UpdateResume(string resumeUrl)
    {
        if (string.IsNullOrWhiteSpace(resumeUrl))
        {
            throw new InvalidOperationException("Resume URL is required.");
        }

        ResumeUrl = resumeUrl;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void RecordRating(decimal newRating)
    {
        var totalScore = (AverageRating * RatingCount) + newRating;
        RatingCount++;
        AverageRating = Math.Round(totalScore / RatingCount, 2);
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
