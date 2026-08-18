namespace PTimeJobs.Domain.Messaging;

public sealed class Conversation
{
    private readonly List<ConversationParticipant> _participants = [];

    private Conversation()
    {
    }

    public Guid ConversationId { get; private set; }
    public ConversationType ConversationType { get; private set; }
    public string? Subject { get; private set; }
    public string? RelatedEntityType { get; private set; }
    public Guid? RelatedEntityId { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? LastMessageAt { get; private set; }

    public IReadOnlyCollection<ConversationParticipant> Participants => _participants.AsReadOnly();

    public static Conversation Create(
        ConversationType conversationType,
        Guid? createdBy = null,
        string? subject = null,
        string? relatedEntityType = null,
        Guid? relatedEntityId = null)
    {
        return new Conversation
        {
            ConversationId = Guid.NewGuid(),
            ConversationType = conversationType,
            Subject = subject,
            RelatedEntityType = relatedEntityType,
            RelatedEntityId = relatedEntityId,
            CreatedBy = createdBy,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void RecordNewMessage()
    {
        LastMessageAt = DateTimeOffset.UtcNow;
    }
}
