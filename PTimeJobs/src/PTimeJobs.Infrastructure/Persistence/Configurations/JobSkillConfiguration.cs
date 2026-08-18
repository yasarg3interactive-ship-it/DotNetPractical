using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobSkillConfiguration : IEntityTypeConfiguration<JobSkill>
{
    public void Configure(EntityTypeBuilder<JobSkill> builder)
    {
        builder.ToTable("job_skills");

        builder.HasKey(jobSkill => new { jobSkill.JobId, jobSkill.SkillId });

        builder.Property(jobSkill => jobSkill.JobId).HasColumnName("job_id");
        builder.Property(jobSkill => jobSkill.SkillId).HasColumnName("skill_id");
        builder.Property(jobSkill => jobSkill.RequiredLevel).HasColumnName("required_level");
        builder.Property(jobSkill => jobSkill.IsMandatory).HasColumnName("is_mandatory");

        builder.HasOne<Job>().WithMany().HasForeignKey(jobSkill => jobSkill.JobId);
        builder.HasOne<Skill>().WithMany().HasForeignKey(jobSkill => jobSkill.SkillId);
    }
}
