using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class CountriesController(ILocationsQueryService queryService, ILocationsCommandService commandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<CountryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var countries = await queryService.GetCountriesAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<CountryResponse>>.Success(countries));
    }

    [HttpGet("{countryId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CountryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CountryResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid countryId, CancellationToken cancellationToken)
    {
        var country = await queryService.GetCountryByIdAsync(countryId, cancellationToken);

        if (country is null)
        {
            return NotFound(ApiResponse<CountryResponse>.Failure("Country not found."));
        }

        return Ok(ApiResponse<CountryResponse>.Success(country));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CountryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCountryRequest request, CancellationToken cancellationToken)
    {
        var country = await commandService.CreateCountryAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { countryId = country.CountryId },
            ApiResponse<CountryResponse>.Success(country, "Country created."));
    }
}
