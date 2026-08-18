using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class RoomsController(IRoomsService roomsService) : ControllerBase
{
    [HttpGet("{roomId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid roomId, CancellationToken cancellationToken)
    {
        var room = await roomsService.GetByIdAsync(roomId, cancellationToken);

        if (room is null)
        {
            return NotFound(ApiResponse<RoomResponse>.Failure("Room not found."));
        }

        return Ok(ApiResponse<RoomResponse>.Success(room));
    }

    [HttpGet("by-property/{propertyId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<RoomResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProperty(Guid propertyId, CancellationToken cancellationToken)
    {
        var rooms = await roomsService.GetByPropertyAsync(propertyId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RoomResponse>>.Success(rooms));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RoomResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoomRequest request, CancellationToken cancellationToken)
    {
        var room = await roomsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { roomId = room.RoomId }, ApiResponse<RoomResponse>.Success(room, "Room created."));
    }

    [HttpGet("{roomId:guid}/availability")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<RoomAvailabilityResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailability(Guid roomId, CancellationToken cancellationToken)
    {
        var availability = await roomsService.GetAvailabilityAsync(roomId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RoomAvailabilityResponse>>.Success(availability));
    }

    [HttpPost("{roomId:guid}/availability")]
    [ProducesResponseType(typeof(ApiResponse<RoomAvailabilityResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<RoomAvailabilityResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddAvailability(Guid roomId, [FromBody] AddRoomAvailabilityRequest request, CancellationToken cancellationToken)
    {
        var availability = await roomsService.AddAvailabilityAsync(roomId, request, cancellationToken);

        if (availability is null)
        {
            return NotFound(ApiResponse<RoomAvailabilityResponse>.Failure("Room not found."));
        }

        return CreatedAtAction(nameof(GetAvailability), new { roomId }, ApiResponse<RoomAvailabilityResponse>.Success(availability, "Availability added."));
    }
}
