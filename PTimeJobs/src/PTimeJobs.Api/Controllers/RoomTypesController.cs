using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class RoomTypesController(IFacilitiesAndRoomTypesService facilitiesAndRoomTypesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<RoomTypeResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var roomTypes = await facilitiesAndRoomTypesService.GetRoomTypesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<RoomTypeResponse>>.Success(roomTypes));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RoomTypeResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRoomTypeRequest request, CancellationToken cancellationToken)
    {
        var roomType = await facilitiesAndRoomTypesService.CreateRoomTypeAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<RoomTypeResponse>.Success(roomType, "Room type created."));
    }
}
