using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RecommendationHistoryConfiguration : IEntityTypeConfiguration<RecommendationHistory>
{
    public void Configure(EntityTypeBuilder<RecommendationHistory> builder)
    {
        builder.ToTable("recommendation_history");

        builder.HasKey(recommendation => recommendation.RecommendationId);

        builder.Property(recommendation => recommendation.RecommendationId).HasColumnName("recommendation_id");
        builder.Property(recommendation => recommendation.UserId).HasColumnName("user_id");
        builder.Property(recommendation => recommendation.RecommendationType).HasColumnName("recommendation_type").HasMaxLength(80);
        builder.Property(recommendation => recommendation.TargetEntityType).HasColumnName("target_entity_type").HasMaxLength(80);
        builder.Property(recommendation => recommendation.TargetEntityId).HasColumnName("target_entity_id");
        builder.Property(recommendation => recommendation.Score).HasColumnName("score").HasColumnType("numeric(6,3)");
        builder.Property(recommendation => recommendation.ModelVersion).HasColumnName("model_version").HasMaxLength(80);
        builder.Property(recommendation => recommendation.Reason).HasColumnName("reason").HasColumnType("jsonb");
        builder.Property(recommendation => recommendation.ShownAt).HasColumnName("shown_at");
        builder.Property(recommendation => recommendation.ClickedAt).HasColumnName("clicked_at");
        builder.Property(recommendation => recommendation.DismissedAt).HasColumnName("dismissed_at");
        builder.Property(recommendation => recommendation.ConvertedAt).HasColumnName("converted_at");

        builder.HasOne<User>().WithMany().HasForeignKey(recommendation => recommendation.UserId);
    }
}
