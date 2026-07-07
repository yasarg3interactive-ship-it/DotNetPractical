using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(user => user.UserId);

        builder.Property(user => user.UserId).HasColumnName("user_id");
        builder.Property(user => user.Email).HasColumnName("email").HasMaxLength(320);
        builder.Property(user => user.MobileNumber).HasColumnName("mobile_number").HasMaxLength(20);
        builder.Property(user => user.PasswordHash).HasColumnName("password_hash");
        builder.Property(user => user.Status)
            .HasColumnName("status")
            .HasColumnType("account_status")
            .HasConversion(
                status => status.ToString().ToLowerInvariant(),
                value => Enum.Parse<AccountStatus>(value, true));
        builder.Property(user => user.IsEmailVerified).HasColumnName("is_email_verified");
        builder.Property(user => user.IsMobileVerified).HasColumnName("is_mobile_verified");
        builder.Property(user => user.LastLoginAt).HasColumnName("last_login_at");
        builder.Property(user => user.LastActiveAt).HasColumnName("last_active_at");
        builder.Property(user => user.FailedLoginCount).HasColumnName("failed_login_count");
        builder.Property(user => user.LockedUntil).HasColumnName("locked_until");
        builder.Property(user => user.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(user => user.CreatedAt).HasColumnName("created_at");
        builder.Property(user => user.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(user => user.Email).IsUnique();
        builder.HasIndex(user => user.MobileNumber).IsUnique();

        builder.Navigation(user => user.UserRoles).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
