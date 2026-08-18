using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.ToTable("user_preferences");

        builder.HasKey(preference => preference.PreferenceId);

        builder.Property(preference => preference.PreferenceId).HasColumnName("preference_id");
        builder.Property(preference => preference.UserId).HasColumnName("user_id");
        builder.Property(preference => preference.PreferenceScope).HasColumnName("preference_scope").HasMaxLength(80);
        builder.Property(preference => preference.Preferences).HasColumnName("preferences").HasColumnType("jsonb");
        builder.Property(preference => preference.CreatedAt).HasColumnName("created_at");
        builder.Property(preference => preference.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(preference => new { preference.UserId, preference.PreferenceScope }).IsUnique();

        builder.HasOne<User>().WithMany().HasForeignKey(preference => preference.UserId);
    }
}
