using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class AccommodationProvidersController(IAccommodationProvidersService accommodationProvidersService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<AccommodationProviderResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var providers = await accommodationProvidersService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<AccommodationProviderResponse>>.Success(providers));
    }

    [HttpGet("{accommodationProviderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationProviderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationProviderResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid accommodationProviderId, CancellationToken cancellationToken)
    {
        var provider = await accommodationProvidersService.GetByIdAsync(accommodationProviderId, cancellationToken);

        if (provider is null)
        {
            return NotFound(ApiResponse<AccommodationProviderResponse>.Failure("Accommodation provider not found."));
        }

        return Ok(ApiResponse<AccommodationProviderResponse>.Success(provider));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<AccommodationProviderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateAccommodationProviderRequest request, CancellationToken cancellationToken)
    {
        var provider = await accommodationProvidersService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { accommodationProviderId = provider.AccommodationProviderId },
            ApiResponse<AccommodationProviderResponse>.Success(provider, "Accommodation provider created."));
    }

    [HttpPatch("{accommodationProviderId:guid}/verify")]
    [ProducesResponseType(typeof(ApiResponse<AccommodationProviderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AccommodationProviderResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkVerified(Guid accommodationProviderId, CancellationToken cancellationToken)
    {
        var provider = await accommodationProvidersService.MarkVerifiedAsync(accommodationProviderId, cancellationToken);

        if (provider is null)
        {
            return NotFound(ApiResponse<AccommodationProviderResponse>.Failure("Accommodation provider not found."));
        }

        return Ok(ApiResponse<AccommodationProviderResponse>.Success(provider, "Provider verified."));
    }
}
