using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("user_sessions");

        builder.HasKey(session => session.SessionId);

        builder.Property(session => session.SessionId).HasColumnName("session_id");
        builder.Property(session => session.UserId).HasColumnName("user_id");
        builder.Property(session => session.RefreshTokenHash).HasColumnName("refresh_token_hash");
        builder.Property(session => session.Status)
            .HasColumnName("status")
            .HasColumnType("session_status");
        builder.Property(session => session.IpAddress).HasColumnName("ip_address").HasColumnType("inet");
        builder.Property(session => session.UserAgent).HasColumnName("user_agent");
        builder.Property(session => session.DeviceId).HasColumnName("device_id").HasMaxLength(120);
        builder.Property(session => session.CreatedAt).HasColumnName("created_at");
        builder.Property(session => session.ExpiresAt).HasColumnName("expires_at");
        builder.Property(session => session.RevokedAt).HasColumnName("revoked_at");

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(session => session.UserId);
    }
}
