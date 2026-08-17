namespace PTimeJobs.Domain.Users;

public sealed class UserRole
{
    private UserRole()
    {
    }

    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }
    public Guid? AssignedBy { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }
    public DateTimeOffset? ExpiresAt { get; private set; }

    public User User { get; private set; } = null!;
    public Role Role { get; private set; } = null!;

    public static UserRole Create(Guid userId, Guid roleId, Guid? assignedBy = null)
    {
        return new UserRole
        {
            UserId = userId,
            RoleId = roleId,
            AssignedBy = assignedBy,
            AssignedAt = DateTimeOffset.UtcNow
        };
    }
}
