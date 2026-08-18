namespace PTimeJobs.Domain.Accommodation;

public sealed class RoomType
{
    private RoomType()
    {
    }

    public Guid RoomTypeId { get; private set; }
    public string TypeName { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public static RoomType Create(string typeName, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(typeName))
        {
            throw new InvalidOperationException("Type name is required.");
        }

        return new RoomType
        {
            RoomTypeId = Guid.NewGuid(),
            TypeName = typeName,
            Description = description
        };
    }
}
