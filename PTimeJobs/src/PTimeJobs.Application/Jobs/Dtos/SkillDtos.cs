namespace PTimeJobs.Application.Jobs.Dtos;

public sealed record SkillResponse(Guid SkillId, string SkillName, string? SkillCategory, bool IsVerified);

public sealed record CreateSkillRequest(string SkillName, string? SkillCategory);
