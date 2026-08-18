using PTimeJobs.Application.Messaging.Dtos;

namespace PTimeJobs.Application.Messaging.Interfaces;

public interface IConversationsService
{
    Task<ConversationResponse?> GetByIdAsync(Guid conversationId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ConversationResponse>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ConversationResponse> CreateAsync(CreateConversationRequest request, CancellationToken cancellationToken = default);

    Task<ConversationResponse?> AddParticipantAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);

    Task<bool> MarkReadAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default);
}
