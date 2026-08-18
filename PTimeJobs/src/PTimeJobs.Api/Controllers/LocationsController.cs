using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class LocationsController(ILocationsQueryService queryService, ILocationsCommandService commandService) : ControllerBase
{
    [HttpGet("{locationId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid locationId, CancellationToken cancellationToken)
    {
        var location = await queryService.GetLocationByIdAsync(locationId, cancellationToken);

        if (location is null)
        {
            return NotFound(ApiResponse<LocationResponse>.Failure("Location not found."));
        }

        return Ok(ApiResponse<LocationResponse>.Success(location));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LocationResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateLocationRequest request, CancellationToken cancellationToken)
    {
        var location = await commandService.CreateLocationAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { locationId = location.LocationId },
            ApiResponse<LocationResponse>.Success(location, "Location created."));
    }
}
