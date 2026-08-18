using Microsoft.AspNetCore.Mvc;
using PTimeJobs.Api.Common;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reports.Dtos;
using PTimeJobs.Application.Reports.Interfaces;

namespace PTimeJobs.Api.Controllers;

[ApiController]
[Route(ApiConstants.BaseRoute)]
public sealed class ReportsController(IReportsService reportsService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ReportResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var reports = await reportsService.GetAllAsync(page, pageSize, cancellationToken);
        return Ok(ApiResponse<PagedResult<ReportResponse>>.Success(reports));
    }

    [HttpGet("{reportId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid reportId, CancellationToken cancellationToken)
    {
        var report = await reportsService.GetByIdAsync(reportId, cancellationToken);

        if (report is null)
        {
            return NotFound(ApiResponse<ReportResponse>.Failure("Report not found."));
        }

        return Ok(ApiResponse<ReportResponse>.Success(report));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateReportRequest request, CancellationToken cancellationToken)
    {
        var report = await reportsService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { reportId = report.ReportId },
            ApiResponse<ReportResponse>.Success(report, "Report queued."));
    }

    [HttpPatch("{reportId:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(Guid reportId, [FromBody] CompleteReportRequest request, CancellationToken cancellationToken)
    {
        var report = await reportsService.CompleteAsync(reportId, request, cancellationToken);

        if (report is null)
        {
            return NotFound(ApiResponse<ReportResponse>.Failure("Report not found."));
        }

        return Ok(ApiResponse<ReportResponse>.Success(report, "Report completed."));
    }

    [HttpPatch("{reportId:guid}/fail")]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ReportResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Fail(Guid reportId, CancellationToken cancellationToken)
    {
        var report = await reportsService.FailAsync(reportId, cancellationToken);

        if (report is null)
        {
            return NotFound(ApiResponse<ReportResponse>.Failure("Report not found."));
        }

        return Ok(ApiResponse<ReportResponse>.Success(report, "Report marked failed."));
    }
}
