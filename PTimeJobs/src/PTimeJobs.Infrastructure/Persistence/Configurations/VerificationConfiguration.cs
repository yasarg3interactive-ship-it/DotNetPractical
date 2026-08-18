using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("verifications");

        builder.HasKey(verification => verification.VerificationId);

        builder.Property(verification => verification.VerificationId).HasColumnName("verification_id");
        builder.Property(verification => verification.UserId).HasColumnName("user_id");
        builder.Property(verification => verification.Channel)
            .HasColumnName("channel")
            .HasColumnType("verification_channel");
        builder.Property(verification => verification.TargetValue).HasColumnName("target_value");
        builder.Property(verification => verification.TokenHash).HasColumnName("token_hash");
        builder.Property(verification => verification.Status)
            .HasColumnName("status")
            .HasColumnType("verification_status");
        builder.Property(verification => verification.RequestedAt).HasColumnName("requested_at");
        builder.Property(verification => verification.VerifiedAt).HasColumnName("verified_at");
        builder.Property(verification => verification.ExpiresAt).HasColumnName("expires_at");
        builder.Property(verification => verification.Metadata).HasColumnName("metadata").HasColumnType("jsonb");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(verification => verification.UserId);
    }
}
