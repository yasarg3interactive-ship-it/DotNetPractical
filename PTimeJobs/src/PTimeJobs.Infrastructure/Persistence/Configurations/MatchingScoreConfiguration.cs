using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class MatchingScoreConfiguration : IEntityTypeConfiguration<MatchingScore>
{
    public void Configure(EntityTypeBuilder<MatchingScore> builder)
    {
        builder.ToTable("matching_scores");

        builder.HasKey(score => score.MatchingScoreId);

        builder.Property(score => score.MatchingScoreId).HasColumnName("matching_score_id");
        builder.Property(score => score.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(score => score.JobId).HasColumnName("job_id");
        builder.Property(score => score.ModelVersion).HasColumnName("model_version").HasMaxLength(80);
        builder.Property(score => score.OverallScore).HasColumnName("overall_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.SkillScore).HasColumnName("skill_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.DistanceScore).HasColumnName("distance_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.AvailabilityScore).HasColumnName("availability_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.ExperienceScore).HasColumnName("experience_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.SalaryScore).HasColumnName("salary_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.RatingScore).HasColumnName("rating_score").HasColumnType("numeric(6,3)");
        builder.Property(score => score.Explanation).HasColumnName("explanation").HasColumnType("jsonb");
        builder.Property(score => score.CalculatedAt).HasColumnName("calculated_at");

        builder.HasIndex(score => new { score.WorkerProfileId, score.JobId, score.ModelVersion }).IsUnique();

        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(score => score.WorkerProfileId);
        builder.HasOne<Job>().WithMany().HasForeignKey(score => score.JobId);
    }
}
