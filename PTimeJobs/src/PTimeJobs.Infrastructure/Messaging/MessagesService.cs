using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Messaging.Dtos;
using PTimeJobs.Application.Messaging.Interfaces;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Messaging;

public sealed class MessagesService(ApplicationDbContext dbContext) : IMessagesService
{
    public async Task<PagedResult<MessageResponse>> GetByConversationAsync(
        Guid conversationId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = dbContext.Messages.AsNoTracking().Where(message => message.ConversationId == conversationId);

        var totalCount = await query.CountAsync(cancellationToken);

        var messages = await query
            .OrderByDescending(message => message.SentAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var messageIds = messages.Select(m => m.MessageId).ToList();
        var attachments = await dbContext.MessageAttachments
            .AsNoTracking()
            .Where(a => messageIds.Contains(a.MessageId))
            .ToListAsync(cancellationToken);

        var items = messages
            .Select(message => ToResponse(message, attachments.Where(a => a.MessageId == message.MessageId).ToList()))
            .ToList();

        return new PagedResult<MessageResponse>(items, page, pageSize, totalCount);
    }

    public async Task<MessageResponse?> SendAsync(Guid conversationId, SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        var conversation = await dbContext.Conversations.FirstOrDefaultAsync(c => c.ConversationId == conversationId, cancellationToken);
        if (conversation is null)
        {
            return null;
        }

        var message = Message.Create(conversationId, request.SenderUserId, request.Body);
        dbContext.Messages.Add(message);

        var attachments = new List<MessageAttachment>();
        if (request.Attachments is not null)
        {
            foreach (var attachmentRequest in request.Attachments)
            {
                var attachment = MessageAttachment.Create(
                    message.MessageId,
                    attachmentRequest.FileUrl,
                    attachmentRequest.FileName,
                    attachmentRequest.MimeType,
                    attachmentRequest.FileSizeBytes);

                attachments.Add(attachment);
                dbContext.MessageAttachments.Add(attachment);
            }
        }

        conversation.RecordNewMessage();
        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(message, attachments);
    }

    public async Task<MessageResponse?> EditAsync(Guid messageId, EditMessageRequest request, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId, cancellationToken);
        if (message is null)
        {
            return null;
        }

        message.Edit(request.Body);
        await dbContext.SaveChangesAsync(cancellationToken);

        var attachments = await dbContext.MessageAttachments
            .AsNoTracking()
            .Where(a => a.MessageId == messageId)
            .ToListAsync(cancellationToken);

        return ToResponse(message, attachments);
    }

    public async Task<MessageResponse?> DeleteAsync(Guid messageId, CancellationToken cancellationToken = default)
    {
        var message = await dbContext.Messages.FirstOrDefaultAsync(m => m.MessageId == messageId, cancellationToken);
        if (message is null)
        {
            return null;
        }

        message.Delete();
        await dbContext.SaveChangesAsync(cancellationToken);

        var attachments = await dbContext.MessageAttachments
            .AsNoTracking()
            .Where(a => a.MessageId == messageId)
            .ToListAsync(cancellationToken);

        return ToResponse(message, attachments);
    }

    private static MessageResponse ToResponse(Message message, IReadOnlyCollection<MessageAttachment> attachments) => new(
        message.MessageId,
        message.ConversationId,
        message.SenderUserId,
        message.Body,
        message.SentAt,
        message.EditedAt,
        message.DeletedAt,
        attachments
            .Select(a => new MessageAttachmentResponse(a.MessageAttachmentId, a.FileUrl, a.FileName, a.MimeType, a.FileSizeBytes))
            .ToList());
}
