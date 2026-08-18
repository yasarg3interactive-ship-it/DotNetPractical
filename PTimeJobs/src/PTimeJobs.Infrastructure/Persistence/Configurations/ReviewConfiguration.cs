using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Reviews;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");

        builder.HasKey(review => review.ReviewId);

        builder.Property(review => review.ReviewId).HasColumnName("review_id");
        builder.Property(review => review.ReviewerUserId).HasColumnName("reviewer_user_id");
        builder.Property(review => review.TargetEntityType).HasColumnName("target_entity_type").HasMaxLength(80);
        builder.Property(review => review.TargetEntityId).HasColumnName("target_entity_id");
        builder.Property(review => review.RelatedEntityType).HasColumnName("related_entity_type").HasMaxLength(80);
        builder.Property(review => review.RelatedEntityId).HasColumnName("related_entity_id");
        builder.Property(review => review.Rating).HasColumnName("rating");
        builder.Property(review => review.ReviewText).HasColumnName("review_text");
        builder.Property(review => review.Status).HasColumnName("status").HasColumnType("review_status");
        builder.Property(review => review.CreatedAt).HasColumnName("created_at");
        builder.Property(review => review.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(review => new
        {
            review.ReviewerUserId,
            review.TargetEntityType,
            review.TargetEntityId,
            review.RelatedEntityType,
            review.RelatedEntityId
        }).IsUnique();

        builder.HasOne<User>().WithMany().HasForeignKey(review => review.ReviewerUserId);
    }
}
