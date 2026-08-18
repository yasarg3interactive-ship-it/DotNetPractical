using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class AreasController(ILocationsQueryService queryService, ILocationsCommandService commandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<AreaResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? cityId, CancellationToken cancellationToken)
    {
        var areas = await queryService.GetAreasAsync(cityId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<AreaResponse>>.Success(areas));
    }

    [HttpGet("{areaId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AreaResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AreaResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid areaId, CancellationToken cancellationToken)
    {
        var area = await queryService.GetAreaByIdAsync(areaId, cancellationToken);

        if (area is null)
        {
            return NotFound(ApiResponse<AreaResponse>.Failure("Area not found."));
        }

        return Ok(ApiResponse<AreaResponse>.Success(area));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AreaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAreaRequest request, CancellationToken cancellationToken)
    {
        var area = await commandService.CreateAreaAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { areaId = area.AreaId },
            ApiResponse<AreaResponse>.Success(area, "Area created."));
    }
}
