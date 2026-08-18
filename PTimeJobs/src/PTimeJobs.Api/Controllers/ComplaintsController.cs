using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Complaints.Dtos;
using PTimeJobs.Application.Complaints.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class ComplaintsController(IComplaintsService complaintsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ComplaintResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var complaints = await complaintsService.SearchAsync(status, page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ComplaintResponse>>.Success(complaints));
    }

    [HttpGet("{complaintId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid complaintId, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.GetByIdAsync(complaintId, cancellationToken);

        if (complaint is null)
        {
            return NotFound(ApiResponse<ComplaintResponse>.Failure("Complaint not found."));
        }

        return Ok(ApiResponse<ComplaintResponse>.Success(complaint));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateComplaintRequest request, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { complaintId = complaint.ComplaintId },
            ApiResponse<ComplaintResponse>.Success(complaint, "Complaint filed."));
    }

    [HttpPatch("{complaintId:guid}/assign/{assignedTo:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Assign(Guid complaintId, Guid assignedTo, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.AssignAsync(complaintId, assignedTo, cancellationToken);

        if (complaint is null)
        {
            return NotFound(ApiResponse<ComplaintResponse>.Failure("Complaint not found."));
        }

        return Ok(ApiResponse<ComplaintResponse>.Success(complaint, "Complaint assigned."));
    }

    [HttpPatch("{complaintId:guid}/resolve")]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resolve(Guid complaintId, [FromBody] ResolveComplaintRequest request, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.ResolveAsync(complaintId, request, cancellationToken);

        if (complaint is null)
        {
            return NotFound(ApiResponse<ComplaintResponse>.Failure("Complaint not found."));
        }

        return Ok(ApiResponse<ComplaintResponse>.Success(complaint, "Complaint resolved."));
    }

    [HttpPatch("{complaintId:guid}/reject")]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(Guid complaintId, [FromBody] ResolveComplaintRequest request, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.RejectAsync(complaintId, request, cancellationToken);

        if (complaint is null)
        {
            return NotFound(ApiResponse<ComplaintResponse>.Failure("Complaint not found."));
        }

        return Ok(ApiResponse<ComplaintResponse>.Success(complaint, "Complaint rejected."));
    }

    [HttpPatch("{complaintId:guid}/escalate")]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ComplaintResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Escalate(Guid complaintId, CancellationToken cancellationToken)
    {
        var complaint = await complaintsService.EscalateAsync(complaintId, cancellationToken);

        if (complaint is null)
        {
            return NotFound(ApiResponse<ComplaintResponse>.Failure("Complaint not found."));
        }

        return Ok(ApiResponse<ComplaintResponse>.Success(complaint, "Complaint escalated."));
    }
}
