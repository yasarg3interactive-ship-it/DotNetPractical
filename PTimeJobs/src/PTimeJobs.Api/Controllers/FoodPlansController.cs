using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Food.Dtos;
using PTimeJobs.Application.Food.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class FoodPlansController(IFoodCatalogService foodCatalogService) : ControllerBase
{
    [HttpGet("by-provider/{foodProviderId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<FoodPlanResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByProvider(Guid foodProviderId, CancellationToken cancellationToken)
    {
        var plans = await foodCatalogService.GetPlansByProviderAsync(foodProviderId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<FoodPlanResponse>>.Success(plans));
    }

    [HttpGet("{foodPlanId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<FoodPlanResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodPlanResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid foodPlanId, CancellationToken cancellationToken)
    {
        var plan = await foodCatalogService.GetPlanByIdAsync(foodPlanId, cancellationToken);

        if (plan is null)
        {
            return NotFound(ApiResponse<FoodPlanResponse>.Failure("Food plan not found."));
        }

        return Ok(ApiResponse<FoodPlanResponse>.Success(plan));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FoodPlanResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFoodPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = await foodCatalogService.CreatePlanAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { foodPlanId = plan.FoodPlanId },
            ApiResponse<FoodPlanResponse>.Success(plan, "Food plan created."));
    }

    [HttpPost("{foodPlanId:guid}/items")]
    [ProducesResponseType(typeof(ApiResponse<FoodPlanResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FoodPlanResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem(Guid foodPlanId, [FromBody] AddFoodPlanItemRequest request, CancellationToken cancellationToken)
    {
        var plan = await foodCatalogService.AddPlanItemAsync(foodPlanId, request, cancellationToken);

        if (plan is null)
        {
            return NotFound(ApiResponse<FoodPlanResponse>.Failure("Food plan not found."));
        }

        return Ok(ApiResponse<FoodPlanResponse>.Success(plan, "Plan item added."));
    }
}
