namespace PTimeJobs.Domain.Jobs;

public sealed class Skill
{
    private Skill()
    {
    }

    public Guid SkillId { get; private set; }
    public string SkillName { get; private set; } = string.Empty;
    public string? SkillCategory { get; private set; }
    public string NormalizedName { get; private set; } = string.Empty;
    public bool IsVerified { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Skill Create(string skillName, string? skillCategory = null)
    {
        if (string.IsNullOrWhiteSpace(skillName))
        {
            throw new InvalidOperationException("Skill name is required.");
        }

        return new Skill
        {
            SkillId = Guid.NewGuid(),
            SkillName = skillName,
            SkillCategory = skillCategory,
            NormalizedName = skillName.Trim().ToLowerInvariant(),
            IsVerified = false,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkVerified()
    {
        IsVerified = true;
    }
}
