namespace PTimeJobs.Application.Messaging.Dtos;

public sealed record ConversationResponse(
    Guid ConversationId,
    string ConversationType,
    string? Subject,
    string? RelatedEntityType,
    Guid? RelatedEntityId,
    Guid? CreatedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastMessageAt,
    IReadOnlyCollection<Guid> ParticipantUserIds);

public sealed record CreateConversationRequest(
    string ConversationType,
    IReadOnlyCollection<Guid> ParticipantUserIds,
    string? Subject,
    string? RelatedEntityType,
    Guid? RelatedEntityId,
    Guid? CreatedBy);

public sealed record AddMessageAttachmentRequest(string FileUrl, string? FileName, string? MimeType, long? FileSizeBytes);

public sealed record MessageAttachmentResponse(Guid MessageAttachmentId, string FileUrl, string? FileName, string? MimeType, long? FileSizeBytes);

public sealed record MessageResponse(
    Guid MessageId,
    Guid ConversationId,
    Guid? SenderUserId,
    string? Body,
    DateTimeOffset SentAt,
    DateTimeOffset? EditedAt,
    DateTimeOffset? DeletedAt,
    IReadOnlyCollection<MessageAttachmentResponse> Attachments);

public sealed record SendMessageRequest(Guid? SenderUserId, string? Body, IReadOnlyCollection<AddMessageAttachmentRequest>? Attachments);

public sealed record EditMessageRequest(string Body);
