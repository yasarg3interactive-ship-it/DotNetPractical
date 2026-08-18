using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class FacilitiesController(IFacilitiesAndRoomTypesService facilitiesAndRoomTypesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<FacilityResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var facilities = await facilitiesAndRoomTypesService.GetFacilitiesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<FacilityResponse>>.Success(facilities));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FacilityResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFacilityRequest request, CancellationToken cancellationToken)
    {
        var facility = await facilitiesAndRoomTypesService.CreateFacilityAsync(request, cancellationToken);
        return Created(string.Empty, ApiResponse<FacilityResponse>.Success(facility, "Facility created."));
    }
}
