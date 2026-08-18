namespace PTimeJobs.Domain.Messaging;

public sealed class MessageAttachment
{
    private MessageAttachment()
    {
    }

    public Guid MessageAttachmentId { get; private set; }
    public Guid MessageId { get; private set; }
    public string FileUrl { get; private set; } = string.Empty;
    public string? FileName { get; private set; }
    public string? MimeType { get; private set; }
    public long? FileSizeBytes { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static MessageAttachment Create(
        Guid messageId,
        string fileUrl,
        string? fileName = null,
        string? mimeType = null,
        long? fileSizeBytes = null)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            throw new InvalidOperationException("File URL is required.");
        }

        return new MessageAttachment
        {
            MessageAttachmentId = Guid.NewGuid(),
            MessageId = messageId,
            FileUrl = fileUrl,
            FileName = fileName,
            MimeType = mimeType,
            FileSizeBytes = fileSizeBytes,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
