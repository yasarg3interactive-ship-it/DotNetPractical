using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable("skills");

        builder.HasKey(skill => skill.SkillId);

        builder.Property(skill => skill.SkillId).HasColumnName("skill_id");
        builder.Property(skill => skill.SkillName).HasColumnName("skill_name").HasMaxLength(120);
        builder.Property(skill => skill.SkillCategory).HasColumnName("skill_category").HasMaxLength(100);
        builder.Property(skill => skill.NormalizedName).HasColumnName("normalized_name").HasMaxLength(120);
        builder.Property(skill => skill.IsVerified).HasColumnName("is_verified");
        builder.Property(skill => skill.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(skill => skill.SkillName).IsUnique();
        builder.HasIndex(skill => skill.NormalizedName).IsUnique();
    }
}
