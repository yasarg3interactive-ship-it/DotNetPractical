namespace PTimeJobs.Application.Users.Dtos;

public sealed record RoleResponse(Guid RoleId, string RoleCode, string RoleName, string? Description, bool IsSystemRole);

public sealed record PermissionResponse(Guid PermissionId, string PermissionCode, string ModuleName, string? Description);

public sealed record CreatePermissionRequest(string PermissionCode, string ModuleName, string? Description);

public sealed record UserRoleResponse(Guid UserId, Guid RoleId, string RoleCode, string RoleName, Guid? AssignedBy, DateTimeOffset AssignedAt, DateTimeOffset? ExpiresAt);

public sealed record AssignRoleRequest(Guid? AssignedBy);
