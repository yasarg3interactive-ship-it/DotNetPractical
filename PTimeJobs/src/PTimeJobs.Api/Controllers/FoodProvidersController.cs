using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class FoodProvidersController(IFoodProvidersService foodProvidersService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<FoodProviderResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var providers = await foodProvidersService.GetAllAsync(cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<FoodProviderResponse>>.Success(providers));
    }

    [HttpGet("{foodProviderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FoodProviderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodProviderResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid foodProviderId, CancellationToken cancellationToken)
    {
        var provider = await foodProvidersService.GetByIdAsync(foodProviderId, cancellationToken);

        if (provider is null)
        {
            return NotFound(ApiResponse<FoodProviderResponse>.Failure("Food provider not found."));
        }

        return Ok(ApiResponse<FoodProviderResponse>.Success(provider));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FoodProviderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFoodProviderRequest request, CancellationToken cancellationToken)
    {
        var provider = await foodProvidersService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { foodProviderId = provider.FoodProviderId },
            ApiResponse<FoodProviderResponse>.Success(provider, "Food provider created."));
    }

    [HttpPatch("{foodProviderId:guid}/verify")]
    [ProducesResponseType(typeof(ApiResponse<FoodProviderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodProviderResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkVerified(Guid foodProviderId, CancellationToken cancellationToken)
    {
        var provider = await foodProvidersService.MarkVerifiedAsync(foodProviderId, cancellationToken);

        if (provider is null)
        {
            return NotFound(ApiResponse<FoodProviderResponse>.Failure("Food provider not found."));
        }

        return Ok(ApiResponse<FoodProviderResponse>.Success(provider, "Food provider verified."));
    }

    [HttpGet("{foodProviderId:guid}/delivery-areas")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<DeliveryAreaResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDeliveryAreas(Guid foodProviderId, CancellationToken cancellationToken)
    {
        var areas = await foodProvidersService.GetDeliveryAreasAsync(foodProviderId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<DeliveryAreaResponse>>.Success(areas));
    }

    [HttpPost("{foodProviderId:guid}/delivery-areas")]
    [ProducesResponseType(typeof(ApiResponse<DeliveryAreaResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<DeliveryAreaResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddDeliveryArea(Guid foodProviderId, [FromBody] CreateDeliveryAreaRequest request, CancellationToken cancellationToken)
    {
        var area = await foodProvidersService.AddDeliveryAreaAsync(foodProviderId, request, cancellationToken);

        if (area is null)
        {
            return NotFound(ApiResponse<DeliveryAreaResponse>.Failure("Food provider not found."));
        }

        return CreatedAtAction(nameof(GetDeliveryAreas), new { foodProviderId }, ApiResponse<DeliveryAreaResponse>.Success(area, "Delivery area added."));
    }
}
