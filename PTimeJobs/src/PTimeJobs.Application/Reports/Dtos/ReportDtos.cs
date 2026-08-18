namespace PTimeJobs.Application.Reports.Dtos;

public sealed record ReportResponse(
    Guid ReportId,
    string ReportType,
    Guid? GeneratedBy,
    string? ReportUrl,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt);

public sealed record CreateReportRequest(string ReportType, Guid? GeneratedBy, string? Parameters);

public sealed record CompleteReportRequest(string ReportUrl);
