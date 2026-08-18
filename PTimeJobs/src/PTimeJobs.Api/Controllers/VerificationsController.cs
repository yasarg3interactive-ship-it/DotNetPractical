using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Users.Dtos;
using PTimeJobs.Application.Users.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class VerificationsController(IVerificationsService verificationsService) : ControllerBase
{
    [HttpGet("{verificationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid verificationId, CancellationToken cancellationToken)
    {
        var verification = await verificationsService.GetByIdAsync(verificationId, cancellationToken);

        if (verification is null)
        {
            return NotFound(ApiResponse<VerificationResponse>.Failure("Verification not found."));
        }

        return Ok(ApiResponse<VerificationResponse>.Success(verification));
    }

    [HttpGet("by-user/{userId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<VerificationResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken cancellationToken)
    {
        var verifications = await verificationsService.GetByUserAsync(userId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<VerificationResponse>>.Success(verifications));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVerificationRequest request, CancellationToken cancellationToken)
    {
        var verification = await verificationsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { verificationId = verification.VerificationId },
            ApiResponse<VerificationResponse>.Success(verification, "Verification requested."));
    }

    [HttpPatch("{verificationId:guid}/verify")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Verify(Guid verificationId, CancellationToken cancellationToken)
    {
        var verification = await verificationsService.VerifyAsync(verificationId, cancellationToken);

        if (verification is null)
        {
            return NotFound(ApiResponse<VerificationResponse>.Failure("Verification not found."));
        }

        return Ok(ApiResponse<VerificationResponse>.Success(verification, "Verified."));
    }

    [HttpPatch("{verificationId:guid}/fail")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkFailed(Guid verificationId, CancellationToken cancellationToken)
    {
        var verification = await verificationsService.MarkFailedAsync(verificationId, cancellationToken);

        if (verification is null)
        {
            return NotFound(ApiResponse<VerificationResponse>.Failure("Verification not found."));
        }

        return Ok(ApiResponse<VerificationResponse>.Success(verification, "Marked failed."));
    }

    [HttpPatch("{verificationId:guid}/revoke")]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<VerificationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Revoke(Guid verificationId, CancellationToken cancellationToken)
    {
        var verification = await verificationsService.RevokeAsync(verificationId, cancellationToken);

        if (verification is null)
        {
            return NotFound(ApiResponse<VerificationResponse>.Failure("Verification not found."));
        }

        return Ok(ApiResponse<VerificationResponse>.Success(verification, "Revoked."));
    }
}
