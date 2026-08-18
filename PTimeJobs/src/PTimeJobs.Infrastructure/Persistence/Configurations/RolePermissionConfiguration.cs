using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions");

        builder.HasKey(rolePermission => new { rolePermission.RoleId, rolePermission.PermissionId });

        builder.Property(rolePermission => rolePermission.RoleId).HasColumnName("role_id");
        builder.Property(rolePermission => rolePermission.PermissionId).HasColumnName("permission_id");

        builder.HasOne<Role>().WithMany().HasForeignKey(rolePermission => rolePermission.RoleId);
        builder.HasOne<Permission>().WithMany().HasForeignKey(rolePermission => rolePermission.PermissionId);
    }
}
