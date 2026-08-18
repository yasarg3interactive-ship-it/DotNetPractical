namespace PTimeJobs.Domain.Jobs;

public sealed class MatchingScore
{
    private MatchingScore()
    {
    }

    public Guid MatchingScoreId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public Guid JobId { get; private set; }
    public string ModelVersion { get; private set; } = string.Empty;
    public decimal OverallScore { get; private set; }
    public decimal? SkillScore { get; private set; }
    public decimal? DistanceScore { get; private set; }
    public decimal? AvailabilityScore { get; private set; }
    public decimal? ExperienceScore { get; private set; }
    public decimal? SalaryScore { get; private set; }
    public decimal? RatingScore { get; private set; }
    public string Explanation { get; private set; } = "{}";
    public DateTimeOffset CalculatedAt { get; private set; }

    public static MatchingScore Create(
        Guid workerProfileId,
        Guid jobId,
        string modelVersion,
        decimal overallScore,
        decimal? skillScore = null,
        decimal? distanceScore = null,
        decimal? availabilityScore = null,
        decimal? experienceScore = null,
        decimal? salaryScore = null,
        decimal? ratingScore = null)
    {
        if (string.IsNullOrWhiteSpace(modelVersion))
        {
            throw new InvalidOperationException("Model version is required.");
        }

        return new MatchingScore
        {
            MatchingScoreId = Guid.NewGuid(),
            WorkerProfileId = workerProfileId,
            JobId = jobId,
            ModelVersion = modelVersion,
            OverallScore = overallScore,
            SkillScore = skillScore,
            DistanceScore = distanceScore,
            AvailabilityScore = availabilityScore,
            ExperienceScore = experienceScore,
            SalaryScore = salaryScore,
            RatingScore = ratingScore,
            CalculatedAt = DateTimeOffset.UtcNow
        };
    }
}
