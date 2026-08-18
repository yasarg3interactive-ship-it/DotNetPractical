using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class FoodItemsController(IFoodCatalogService foodCatalogService) : ControllerBase
{
    [HttpGet("by-provider/{foodProviderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<FoodItemResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProvider(Guid foodProviderId, CancellationToken cancellationToken)
    {
        var items = await foodCatalogService.GetItemsByProviderAsync(foodProviderId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<FoodItemResponse>>.Success(items));
    }

    [HttpGet("{foodItemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FoodItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodItemResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid foodItemId, CancellationToken cancellationToken)
    {
        var item = await foodCatalogService.GetItemByIdAsync(foodItemId, cancellationToken);

        if (item is null)
        {
            return NotFound(ApiResponse<FoodItemResponse>.Failure("Food item not found."));
        }

        return Ok(ApiResponse<FoodItemResponse>.Success(item));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FoodItemResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFoodItemRequest request, CancellationToken cancellationToken)
    {
        var item = await foodCatalogService.CreateItemAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { foodItemId = item.FoodItemId },
            ApiResponse<FoodItemResponse>.Success(item, "Food item created."));
    }
}
