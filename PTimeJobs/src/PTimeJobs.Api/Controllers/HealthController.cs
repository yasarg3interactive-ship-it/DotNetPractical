using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Interfaces;
using PTimeJobs.Application.Common.Models;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class HealthController(IDatabaseConnectionChecker databaseConnectionChecker) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var databaseConnected = await databaseConnectionChecker.CanConnectAsync(cancellationToken);
        var data = new
        {
            service = "PTimeJobs API",
            databaseConnected,
            checkedAt = DateTimeOffset.UtcNow
        };

        if (!databaseConnected)
        {
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                ApiResponse<object>.Failure("Database connection failed."));
        }

        return Ok(ApiResponse<object>.Success(data, "Service is healthy."));
    }
}
