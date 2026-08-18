using Microsoft.EntityFrameworkCore;
using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reports.Dtos;
using PTimeJobs.Application.Reports.Interfaces;
using PTimeJobs.Domain.Reports;
using PTimeJobs.Infrastructure.Persistence;

namespace PTimeJobs.Infrastructure.Reports;

public sealed class ReportsService(ApplicationDbContext dbContext) : IReportsService
{
    public async Task<ReportResponse?> GetByIdAsync(Guid reportId, CancellationToken cancellationToken = default)
    {
        var report = await dbContext.Reports.AsNoTracking().FirstOrDefaultAsync(r => r.ReportId == reportId, cancellationToken);
        return report is null ? null : ToResponse(report);
    }

    public async Task<PagedResult<ReportResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

        var query = dbContext.Reports.AsNoTracking();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<ReportResponse>(items.Select(ToResponse).ToList(), page, pageSize, totalCount);
    }

    public async Task<ReportResponse> CreateAsync(CreateReportRequest request, CancellationToken cancellationToken = default)
    {
        var report = Report.Create(request.ReportType, request.GeneratedBy, request.Parameters ?? "{}");
        dbContext.Reports.Add(report);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(report);
    }

    public async Task<ReportResponse?> CompleteAsync(Guid reportId, CompleteReportRequest request, CancellationToken cancellationToken = default)
    {
        var report = await dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == reportId, cancellationToken);
        if (report is null)
        {
            return null;
        }

        report.Complete(request.ReportUrl);
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(report);
    }

    public async Task<ReportResponse?> FailAsync(Guid reportId, CancellationToken cancellationToken = default)
    {
        var report = await dbContext.Reports.FirstOrDefaultAsync(r => r.ReportId == reportId, cancellationToken);
        if (report is null)
        {
            return null;
        }

        report.Fail();
        await dbContext.SaveChangesAsync(cancellationToken);
        return ToResponse(report);
    }

    private static ReportResponse ToResponse(Report report) => new(
        report.ReportId,
        report.ReportType,
        report.GeneratedBy,
        report.ReportUrl,
        report.Status,
        report.CreatedAt,
        report.CompletedAt);
}
