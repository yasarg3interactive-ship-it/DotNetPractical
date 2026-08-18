using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;
using PTimeJobs.Domain.Users;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Users;

public sealed class RbacService(ApplicationDbContext dbContext) : IRbacService
{
    public async Task<IReadOnlyCollection<RoleResponse>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .OrderBy(role => role.RoleName)
            .Select(role => new RoleResponse(role.RoleId, role.RoleCode, role.RoleName, role.Description, role.IsSystemRole))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleResponse?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Roles
            .AsNoTracking()
            .Where(role => role.RoleId == roleId)
            .Select(role => new RoleResponse(role.RoleId, role.RoleCode, role.RoleName, role.Description, role.IsSystemRole))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<PermissionResponse>> GetPermissionsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .OrderBy(permission => permission.ModuleName)
            .ThenBy(permission => permission.PermissionCode)
            .Select(permission => new PermissionResponse(permission.PermissionId, permission.PermissionCode, permission.ModuleName, permission.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<PermissionResponse?> GetPermissionByIdAsync(Guid permissionId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Permissions
            .AsNoTracking()
            .Where(permission => permission.PermissionId == permissionId)
            .Select(permission => new PermissionResponse(permission.PermissionId, permission.PermissionCode, permission.ModuleName, permission.Description))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default)
    {
        var duplicate = await dbContext.Permissions
            .AsNoTracking()
            .AnyAsync(permission => permission.PermissionCode == request.PermissionCode, cancellationToken);

        if (duplicate)
        {
            throw new InvalidOperationException("A permission with this code already exists.");
        }

        var permission = Permission.Create(request.PermissionCode, request.ModuleName, request.Description);
        dbContext.Permissions.Add(permission);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PermissionResponse(permission.PermissionId, permission.PermissionCode, permission.ModuleName, permission.Description);
    }

    public async Task<IReadOnlyCollection<PermissionResponse>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default)
    {
        return await (
            from rolePermission in dbContext.RolePermissions.AsNoTracking()
            where rolePermission.RoleId == roleId
            join permission in dbContext.Permissions.AsNoTracking() on rolePermission.PermissionId equals permission.PermissionId
            select new PermissionResponse(permission.PermissionId, permission.PermissionCode, permission.ModuleName, permission.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default)
    {
        var roleExists = await dbContext.Roles.AsNoTracking().AnyAsync(role => role.RoleId == roleId, cancellationToken);
        if (!roleExists)
        {
            return false;
        }

        var permissionExists = await dbContext.Permissions.AsNoTracking().AnyAsync(p => p.PermissionId == permissionId, cancellationToken);
        if (!permissionExists)
        {
            throw new InvalidOperationException("Permission not found.");
        }

        var alreadyAssigned = await dbContext.RolePermissions
            .AsNoTracking()
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId, cancellationToken);

        if (!alreadyAssigned)
        {
            dbContext.RolePermissions.Add(RolePermission.Create(roleId, permissionId));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return true;
    }

    public async Task<IReadOnlyCollection<UserRoleResponse>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await (
            from userRole in dbContext.UserRoles.AsNoTracking()
            where userRole.UserId == userId
            join role in dbContext.Roles.AsNoTracking() on userRole.RoleId equals role.RoleId
            select new UserRoleResponse(
                userRole.UserId,
                userRole.RoleId,
                role.RoleCode,
                role.RoleName,
                userRole.AssignedBy,
                userRole.AssignedAt,
                userRole.ExpiresAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserRoleResponse?> AssignRoleToUserAsync(
        Guid userId,
        Guid roleId,
        AssignRoleRequest request,
        CancellationToken cancellationToken = default)
    {
        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == userId, cancellationToken);
        if (!userExists)
        {
            return null;
        }

        var role = await dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
        if (role is null)
        {
            throw new InvalidOperationException("Role not found.");
        }

        var alreadyAssigned = await dbContext.UserRoles
            .AsNoTracking()
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);

        if (alreadyAssigned)
        {
            throw new InvalidOperationException("This role is already assigned to the user.");
        }

        var userRole = UserRole.Create(userId, roleId, request.AssignedBy);
        dbContext.UserRoles.Add(userRole);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UserRoleResponse(userRole.UserId, userRole.RoleId, role.RoleCode, role.RoleName, userRole.AssignedBy, userRole.AssignedAt, userRole.ExpiresAt);
    }

    public async Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default)
    {
        var userRole = await dbContext.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId, cancellationToken);
        if (userRole is null)
        {
            return false;
        }

        dbContext.UserRoles.Remove(userRole);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
