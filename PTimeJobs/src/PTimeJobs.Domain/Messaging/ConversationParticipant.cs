namespace PTimeJobs.Domain.Messaging;

public sealed class ConversationParticipant
{
    private ConversationParticipant()
    {
    }

    public Guid ConversationId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public DateTimeOffset? LastReadAt { get; private set; }
    public bool IsMuted { get; private set; }

    public static ConversationParticipant Create(Guid conversationId, Guid userId)
    {
        return new ConversationParticipant
        {
            ConversationId = conversationId,
            UserId = userId,
            JoinedAt = DateTimeOffset.UtcNow,
            IsMuted = false
        };
    }

    public void MarkRead()
    {
        LastReadAt = DateTimeOffset.UtcNow;
    }

    public void Mute()
    {
        IsMuted = true;
    }

    public void Unmute()
    {
        IsMuted = false;
    }
}
