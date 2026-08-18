using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Messaging.Dtos;
using PTimeJobs.Application.Messaging.Interfaces;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Messaging;

public sealed class ConversationsService(ApplicationDbContext dbContext) : IConversationsService
{
    public async Task<ConversationResponse?> GetByIdAsync(Guid conversationId, CancellationToken cancellationToken = default)
    {
        var conversation = await dbContext.Conversations
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ConversationId == conversationId, cancellationToken);

        if (conversation is null)
        {
            return null;
        }

        var participantIds = await dbContext.ConversationParticipants
            .AsNoTracking()
            .Where(p => p.ConversationId == conversationId)
            .Select(p => p.UserId)
            .ToListAsync(cancellationToken);

        return ToResponse(conversation, participantIds);
    }

    public async Task<IReadOnlyCollection<ConversationResponse>> GetForUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var conversationIds = await dbContext.ConversationParticipants
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => p.ConversationId)
            .ToListAsync(cancellationToken);

        var conversations = await dbContext.Conversations
            .AsNoTracking()
            .Where(c => conversationIds.Contains(c.ConversationId))
            .OrderByDescending(c => c.LastMessageAt ?? c.CreatedAt)
            .ToListAsync(cancellationToken);

        var allParticipants = await dbContext.ConversationParticipants
            .AsNoTracking()
            .Where(p => conversationIds.Contains(p.ConversationId))
            .ToListAsync(cancellationToken);

        return conversations
            .Select(c => ToResponse(c, allParticipants.Where(p => p.ConversationId == c.ConversationId).Select(p => p.UserId).ToList()))
            .ToList();
    }

    public async Task<ConversationResponse> CreateAsync(CreateConversationRequest request, CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<ConversationType>(request.ConversationType, true, out var conversationType))
        {
            throw new InvalidOperationException($"Unknown conversation type '{request.ConversationType}'.");
        }

        if (request.ParticipantUserIds is null || request.ParticipantUserIds.Count == 0)
        {
            throw new InvalidOperationException("At least one participant is required.");
        }

        var distinctUserIds = request.ParticipantUserIds.Distinct().ToList();
        var existingUserCount = await dbContext.Users
            .AsNoTracking()
            .CountAsync(user => distinctUserIds.Contains(user.UserId), cancellationToken);

        if (existingUserCount != distinctUserIds.Count)
        {
            throw new InvalidOperationException("One or more participant users were not found.");
        }

        var conversation = Conversation.Create(
            conversationType,
            request.CreatedBy,
            request.Subject,
            request.RelatedEntityType,
            request.RelatedEntityId);

        dbContext.Conversations.Add(conversation);

        foreach (var userId in distinctUserIds)
        {
            dbContext.ConversationParticipants.Add(ConversationParticipant.Create(conversation.ConversationId, userId));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return ToResponse(conversation, distinctUserIds);
    }

    public async Task<ConversationResponse?> AddParticipantAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var conversationExists = await dbContext.Conversations
            .AsNoTracking()
            .AnyAsync(c => c.ConversationId == conversationId, cancellationToken);

        if (!conversationExists)
        {
            return null;
        }

        var userExists = await dbContext.Users.AsNoTracking().AnyAsync(user => user.UserId == userId, cancellationToken);
        if (!userExists)
        {
            throw new InvalidOperationException("User not found.");
        }

        var alreadyParticipant = await dbContext.ConversationParticipants
            .AsNoTracking()
            .AnyAsync(p => p.ConversationId == conversationId && p.UserId == userId, cancellationToken);

        if (!alreadyParticipant)
        {
            dbContext.ConversationParticipants.Add(ConversationParticipant.Create(conversationId, userId));
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return await GetByIdAsync(conversationId, cancellationToken);
    }

    public async Task<bool> MarkReadAsync(Guid conversationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var participant = await dbContext.ConversationParticipants
            .FirstOrDefaultAsync(p => p.ConversationId == conversationId && p.UserId == userId, cancellationToken);

        if (participant is null)
        {
            return false;
        }

        participant.MarkRead();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static ConversationResponse ToResponse(Conversation conversation, IReadOnlyCollection<Guid> participantIds) => new(
        conversation.ConversationId,
        conversation.ConversationType.ToString(),
        conversation.Subject,
        conversation.RelatedEntityType,
        conversation.RelatedEntityId,
        conversation.CreatedBy,
        conversation.CreatedAt,
        conversation.LastMessageAt,
        participantIds);
}
