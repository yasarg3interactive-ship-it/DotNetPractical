using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.RoleId);

        builder.Property(role => role.RoleId).HasColumnName("role_id");
        builder.Property(role => role.RoleCode).HasColumnName("role_code").HasMaxLength(60);
        builder.Property(role => role.RoleName).HasColumnName("role_name").HasMaxLength(120);
        builder.Property(role => role.Description).HasColumnName("description");
        builder.Property(role => role.IsSystemRole).HasColumnName("is_system_role");
        builder.Property(role => role.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(role => role.RoleCode).IsUnique();

        builder.Navigation(role => role.UserRoles).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
