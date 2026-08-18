using PTimeJobs.Application.Common.Models;
using PTimeJobs.Application.Reports.Dtos;

namespace PTimeJobs.Application.Reports.Interfaces;

public interface IReportsService
{
    Task<ReportResponse?> GetByIdAsync(Guid reportId, CancellationToken cancellationToken = default);

    Task<PagedResult<ReportResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);

    Task<ReportResponse> CreateAsync(CreateReportRequest request, CancellationToken cancellationToken = default);

    Task<ReportResponse?> CompleteAsync(Guid reportId, CompleteReportRequest request, CancellationToken cancellationToken = default);

    Task<ReportResponse?> FailAsync(Guid reportId, CancellationToken cancellationToken = default);
}
