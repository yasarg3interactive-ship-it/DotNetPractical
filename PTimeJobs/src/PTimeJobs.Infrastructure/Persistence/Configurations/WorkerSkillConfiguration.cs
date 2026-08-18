using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerSkillConfiguration : IEntityTypeConfiguration<WorkerSkill>
{
    public void Configure(EntityTypeBuilder<WorkerSkill> builder)
    {
        builder.ToTable("worker_skills");

        builder.HasKey(workerSkill => new { workerSkill.WorkerProfileId, workerSkill.SkillId });

        builder.Property(workerSkill => workerSkill.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(workerSkill => workerSkill.SkillId).HasColumnName("skill_id");
        builder.Property(workerSkill => workerSkill.ProficiencyLevel).HasColumnName("proficiency_level");
        builder.Property(workerSkill => workerSkill.YearsExperience).HasColumnName("years_experience").HasColumnType("numeric(4,1)");
        builder.Property(workerSkill => workerSkill.IsPrimary).HasColumnName("is_primary");
        builder.Property(workerSkill => workerSkill.VerifiedAt).HasColumnName("verified_at");

        builder.HasOne<WorkerProfile>()
            .WithMany(profile => profile.Skills)
            .HasForeignKey(workerSkill => workerSkill.WorkerProfileId);

        builder.HasOne<Skill>().WithMany().HasForeignKey(workerSkill => workerSkill.SkillId);
    }
}
