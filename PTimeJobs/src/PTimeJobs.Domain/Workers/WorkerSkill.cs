namespace PTimeJobs.Domain.Workers;

public sealed class WorkerSkill
{
    private WorkerSkill()
    {
    }

    public Guid WorkerProfileId { get; private set; }
    public Guid SkillId { get; private set; }
    public short ProficiencyLevel { get; private set; }
    public decimal? YearsExperience { get; private set; }
    public bool IsPrimary { get; private set; }
    public DateTimeOffset? VerifiedAt { get; private set; }

    public static WorkerSkill Create(
        Guid workerProfileId,
        Guid skillId,
        short proficiencyLevel = 1,
        decimal? yearsExperience = null,
        bool isPrimary = false)
    {
        if (proficiencyLevel is < 1 or > 5)
        {
            throw new InvalidOperationException("Proficiency level must be between 1 and 5.");
        }

        return new WorkerSkill
        {
            WorkerProfileId = workerProfileId,
            SkillId = skillId,
            ProficiencyLevel = proficiencyLevel,
            YearsExperience = yearsExperience,
            IsPrimary = isPrimary
        };
    }

    public void MarkVerified()
    {
        VerifiedAt = DateTimeOffset.UtcNow;
    }
}
