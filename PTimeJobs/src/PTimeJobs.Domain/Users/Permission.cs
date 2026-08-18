namespace PTimeJobs.Domain.Users;

public sealed class Permission
{
    private Permission()
    {
    }

    public Guid PermissionId { get; private set; }
    public string PermissionCode { get; private set; } = string.Empty;
    public string ModuleName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Permission Create(string permissionCode, string moduleName, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(permissionCode))
        {
            throw new InvalidOperationException("Permission code is required.");
        }

        if (string.IsNullOrWhiteSpace(moduleName))
        {
            throw new InvalidOperationException("Module name is required.");
        }

        return new Permission
        {
            PermissionId = Guid.NewGuid(),
            PermissionCode = permissionCode,
            ModuleName = moduleName,
            Description = description,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
