namespace PTimeJobs.Domain.Jobs;

public sealed class Shortlist
{
    private Shortlist()
    {
    }

    public Guid ShortlistId { get; private set; }
    public Guid ApplicationId { get; private set; }
    public Guid? ShortlistedBy { get; private set; }
    public string? Notes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static Shortlist Create(Guid applicationId, Guid? shortlistedBy = null, string? notes = null)
    {
        return new Shortlist
        {
            ShortlistId = Guid.NewGuid(),
            ApplicationId = applicationId,
            ShortlistedBy = shortlistedBy,
            Notes = notes,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
