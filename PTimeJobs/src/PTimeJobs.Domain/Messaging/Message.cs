namespace PTimeJobs.Domain.Messaging;

public sealed class Message
{
    private Message()
    {
    }

    public Guid MessageId { get; private set; }
    public Guid ConversationId { get; private set; }
    public Guid? SenderUserId { get; private set; }
    public string? Body { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset SentAt { get; private set; }
    public DateTimeOffset? EditedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public static Message Create(Guid conversationId, Guid? senderUserId, string? body)
    {
        return new Message
        {
            MessageId = Guid.NewGuid(),
            ConversationId = conversationId,
            SenderUserId = senderUserId,
            Body = body,
            SentAt = DateTimeOffset.UtcNow
        };
    }

    public void Edit(string newBody)
    {
        Body = newBody;
        EditedAt = DateTimeOffset.UtcNow;
    }

    public void Delete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
