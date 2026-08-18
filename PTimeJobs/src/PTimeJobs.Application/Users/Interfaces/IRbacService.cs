using PTimeJobs.Application.Users.Dtos;

namespace PTimeJobs.Application.Users.Interfaces;

public interface IRbacService
{
    Task<IReadOnlyCollection<RoleResponse>> GetRolesAsync(CancellationToken cancellationToken = default);

    Task<RoleResponse?> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PermissionResponse>> GetPermissionsAsync(CancellationToken cancellationToken = default);

    Task<PermissionResponse?> GetPermissionByIdAsync(Guid permissionId, CancellationToken cancellationToken = default);

    Task<PermissionResponse> CreatePermissionAsync(CreatePermissionRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PermissionResponse>> GetRolePermissionsAsync(Guid roleId, CancellationToken cancellationToken = default);

    Task<bool> AssignPermissionToRoleAsync(Guid roleId, Guid permissionId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserRoleResponse>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserRoleResponse?> AssignRoleToUserAsync(Guid userId, Guid roleId, AssignRoleRequest request, CancellationToken cancellationToken = default);

    Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId, CancellationToken cancellationToken = default);
}
