using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Messaging.Dtos;

namespace PTimeJobs.Application.Messaging.Interfaces;

public interface IMessagesService
{
    Task<PagedResult<MessageResponse>> GetByConversationAsync(
        Guid conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<MessageResponse?> SendAsync(Guid conversationId, SendMessageRequest request, CancellationToken cancellationToken = default);

    Task<MessageResponse?> EditAsync(Guid messageId, EditMessageRequest request, CancellationToken cancellationToken = default);

    Task<MessageResponse?> DeleteAsync(Guid messageId, CancellationToken cancellationToken = default);
}
