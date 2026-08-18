using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Accommodation.Dtos;
using PTimeJobs.Application.Accommodation.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class PropertiesController(IPropertiesService propertiesService) : ControllerBase
{
    [HttpGet("{propertyId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid propertyId, CancellationToken cancellationToken)
    {
        var property = await propertiesService.GetByIdAsync(propertyId, cancellationToken);

        if (property is null)
        {
            return NotFound(ApiResponse<PropertyResponse>.Failure("Property not found."));
        }

        return Ok(ApiResponse<PropertyResponse>.Success(property));
    }

    [HttpGet("by-provider/{accommodationProviderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<PropertyResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProvider(Guid accommodationProviderId, CancellationToken cancellationToken)
    {
        var properties = await propertiesService.GetByProviderAsync(accommodationProviderId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<PropertyResponse>>.Success(properties));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePropertyRequest request, CancellationToken cancellationToken)
    {
        var property = await propertiesService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { propertyId = property.PropertyId },
            ApiResponse<PropertyResponse>.Success(property, "Property created."));
    }

    [HttpPatch("{propertyId:guid}/deactivate")]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(Guid propertyId, CancellationToken cancellationToken)
    {
        var property = await propertiesService.DeactivateAsync(propertyId, cancellationToken);

        if (property is null)
        {
            return NotFound(ApiResponse<PropertyResponse>.Failure("Property not found."));
        }

        return Ok(ApiResponse<PropertyResponse>.Success(property, "Property deactivated."));
    }

    [HttpPost("{propertyId:guid}/images")]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddImage(Guid propertyId, [FromBody] AddPropertyImageRequest request, CancellationToken cancellationToken)
    {
        var property = await propertiesService.AddImageAsync(propertyId, request, cancellationToken);

        if (property is null)
        {
            return NotFound(ApiResponse<PropertyResponse>.Failure("Property not found."));
        }

        return Ok(ApiResponse<PropertyResponse>.Success(property, "Image added."));
    }

    [HttpPost("{propertyId:guid}/facilities")]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<PropertyResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFacility(Guid propertyId, [FromBody] AddPropertyFacilityRequest request, CancellationToken cancellationToken)
    {
        var property = await propertiesService.AddFacilityAsync(propertyId, request, cancellationToken);

        if (property is null)
        {
            return NotFound(ApiResponse<PropertyResponse>.Failure("Property not found."));
        }

        return Ok(ApiResponse<PropertyResponse>.Success(property, "Facility added."));
    }
}
