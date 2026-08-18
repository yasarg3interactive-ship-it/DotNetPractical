using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Messaging.Dtos;
using PTimeJobs.Application.Messaging.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class MessagesController(IMessagesService messagesService) : ControllerBase
{
    [HttpGet("by-conversation/{conversationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MessageResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByConversation(
        Guid conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var messages = await messagesService.GetByConversationAsync(conversationId, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<MessageResponse>>.Success(messages));
    }

    [HttpPost("conversation/{conversationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Send(Guid conversationId, [FromBody] SendMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await messagesService.SendAsync(conversationId, request, cancellationToken);

        if (message is null)
        {
            return NotFound(ApiResponse<MessageResponse>.Failure("Conversation not found."));
        }

        return CreatedAtAction(
            nameof(GetByConversation),
            new { conversationId },
            ApiResponse<MessageResponse>.Success(message, "Message sent."));
    }

    [HttpPatch("{messageId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(Guid messageId, [FromBody] EditMessageRequest request, CancellationToken cancellationToken)
    {
        var message = await messagesService.EditAsync(messageId, request, cancellationToken);

        if (message is null)
        {
            return NotFound(ApiResponse<MessageResponse>.Failure("Message not found."));
        }

        return Ok(ApiResponse<MessageResponse>.Success(message, "Message edited."));
    }

    [HttpDelete("{messageId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<MessageResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid messageId, CancellationToken cancellationToken)
    {
        var message = await messagesService.DeleteAsync(messageId, cancellationToken);

        if (message is null)
        {
            return NotFound(ApiResponse<MessageResponse>.Failure("Message not found."));
        }

        return Ok(ApiResponse<MessageResponse>.Success(message, "Message deleted."));
    }
}
