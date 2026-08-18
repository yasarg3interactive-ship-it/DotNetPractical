namespace PTimeJobs.Domain.Workers;

public sealed class WorkerEducation
{
    private WorkerEducation()
    {
    }

    public Guid EducationId { get; private set; }
    public Guid WorkerProfileId { get; private set; }
    public string InstitutionName { get; private set; } = string.Empty;
    public string? Degree { get; private set; }
    public string? FieldOfStudy { get; private set; }
    public short? StartYear { get; private set; }
    public short? EndYear { get; private set; }
    public bool IsCurrent { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static WorkerEducation Create(
        Guid workerProfileId,
        string institutionName,
        string? degree = null,
        string? fieldOfStudy = null,
        short? startYear = null,
        short? endYear = null,
        bool isCurrent = false)
    {
        if (string.IsNullOrWhiteSpace(institutionName))
        {
            throw new InvalidOperationException("Institution name is required.");
        }

        return new WorkerEducation
        {
            EducationId = Guid.NewGuid(),
            WorkerProfileId = workerProfileId,
            InstitutionName = institutionName,
            Degree = degree,
            FieldOfStudy = fieldOfStudy,
            StartYear = startYear,
            EndYear = endYear,
            IsCurrent = isCurrent,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
