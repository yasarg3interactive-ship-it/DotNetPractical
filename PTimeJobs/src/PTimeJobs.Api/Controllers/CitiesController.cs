using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class CitiesController(ILocationsQueryService queryService, ILocationsCommandService commandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<CityResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? stateId, CancellationToken cancellationToken)
    {
        var cities = await queryService.GetCitiesAsync(stateId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<CityResponse>>.Success(cities));
    }

    [HttpGet("{cityId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CityResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CityResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid cityId, CancellationToken cancellationToken)
    {
        var city = await queryService.GetCityByIdAsync(cityId, cancellationToken);

        if (city is null)
        {
            return NotFound(ApiResponse<CityResponse>.Failure("City not found."));
        }

        return Ok(ApiResponse<CityResponse>.Success(city));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CityResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCityRequest request, CancellationToken cancellationToken)
    {
        var city = await commandService.CreateCityAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { cityId = city.CityId },
            ApiResponse<CityResponse>.Success(city, "City created."));
    }
}
