namespace PTimeJobs.Domain.Jobs;

public sealed class JobSkill
{
    private JobSkill()
    {
    }

    public Guid JobId { get; private set; }
    public Guid SkillId { get; private set; }
    public short RequiredLevel { get; private set; }
    public bool IsMandatory { get; private set; }

    public static JobSkill Create(Guid jobId, Guid skillId, short requiredLevel = 1, bool isMandatory = true)
    {
        if (requiredLevel is < 1 or > 5)
        {
            throw new InvalidOperationException("Required level must be between 1 and 5.");
        }

        return new JobSkill
        {
            JobId = jobId,
            SkillId = skillId,
            RequiredLevel = requiredLevel,
            IsMandatory = isMandatory
        };
    }
}
