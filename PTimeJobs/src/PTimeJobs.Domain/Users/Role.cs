namespace PTimeJobs.Domain.Users;

public sealed class Role
{
    private readonly List<UserRole> _userRoles = [];

    private Role()
    {
    }

    public Guid RoleId { get; private set; }
    public string RoleCode { get; private set; } = string.Empty;
    public string RoleName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsSystemRole { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();
}
