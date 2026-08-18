using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(permission => permission.PermissionId);

        builder.Property(permission => permission.PermissionId).HasColumnName("permission_id");
        builder.Property(permission => permission.PermissionCode).HasColumnName("permission_code").HasMaxLength(120);
        builder.Property(permission => permission.ModuleName).HasColumnName("module_name").HasMaxLength(80);
        builder.Property(permission => permission.Description).HasColumnName("description");
        builder.Property(permission => permission.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(permission => permission.PermissionCode).IsUnique();
    }
}
