using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Locations.Dtos;
using PTimeJobs.Application.Locations.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class StatesController(ILocationsQueryService queryService, ILocationsCommandService commandService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyCollection<StateResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] Guid? countryId, CancellationToken cancellationToken)
    {
        var states = await queryService.GetStatesAsync(countryId, cancellationToken);
        return Ok(ApiResponse<IReadOnlyCollection<StateResponse>>.Success(states));
    }

    [HttpGet("{stateId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<StateResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<StateResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid stateId, CancellationToken cancellationToken)
    {
        var state = await queryService.GetStateByIdAsync(stateId, cancellationToken);

        if (state is null)
        {
            return NotFound(ApiResponse<StateResponse>.Failure("State not found."));
        }

        return Ok(ApiResponse<StateResponse>.Success(state));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<StateResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStateRequest request, CancellationToken cancellationToken)
    {
        var state = await commandService.CreateStateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { stateId = state.StateId },
            ApiResponse<StateResponse>.Success(state, "State created."));
    }
}
