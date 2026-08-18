using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Messaging.Dtos;
using PTimeJobs.Application.Messaging.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class ConversationsController(IConversationsService conversationsService) : ControllerBase
{
    [HttpGet("{conversationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid conversationId, CancellationToken cancellationToken)
    {
        var conversation = await conversationsService.GetByIdAsync(conversationId, cancellationToken);

        if (conversation is null)
        {
            return NotFound(ApiResponse<ConversationResponse>.Failure("Conversation not found."));
        }

        return Ok(ApiResponse<ConversationResponse>.Success(conversation));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<ConversationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetForUser(Guid userId, CancellationToken cancellationToken)
    {
        var conversations = await conversationsService.GetForUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<ConversationResponse>>.Success(conversations));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateConversationRequest request, CancellationToken cancellationToken)
    {
        var conversation = await conversationsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { conversationId = conversation.ConversationId },
            ApiResponse<ConversationResponse>.Success(conversation, "Conversation created."));
    }

    [HttpPost("{conversationId:guid}/participants/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ConversationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddParticipant(Guid conversationId, Guid userId, CancellationToken cancellationToken)
    {
        var conversation = await conversationsService.AddParticipantAsync(conversationId, userId, cancellationToken);

        if (conversation is null)
        {
            return NotFound(ApiResponse<ConversationResponse>.Failure("Conversation not found."));
        }

        return Ok(ApiResponse<ConversationResponse>.Success(conversation, "Participant added."));
    }

    [HttpPatch("{conversationId:guid}/participants/{userId:guid}/mark-read")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkRead(Guid conversationId, Guid userId, CancellationToken cancellationToken)
    {
        var marked = await conversationsService.MarkReadAsync(conversationId, userId, cancellationToken);

        if (!marked)
        {
            return NotFound(ApiResponse<object>.Failure("Participant not found in this conversation."));
        }

        return Ok(ApiResponse<object>.Success(null, "Marked as read."));
    }
}
