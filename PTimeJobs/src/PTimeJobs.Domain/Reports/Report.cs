namespace PTimeJobs.Domain.Reports;

public sealed class Report
{
    private Report()
    {
    }

    public Guid ReportId { get; private set; }
    public string ReportType { get; private set; } = string.Empty;
    public Guid? GeneratedBy { get; private set; }
    public string Parameters { get; private set; } = "{}";
    public string? ReportUrl { get; private set; }
    public string Status { get; private set; } = "queued";
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }

    public static Report Create(string reportType, Guid? generatedBy = null, string parameters = "{}")
    {
        if (string.IsNullOrWhiteSpace(reportType))
        {
            throw new InvalidOperationException("Report type is required.");
        }

        return new Report
        {
            ReportId = Guid.NewGuid(),
            ReportType = reportType,
            GeneratedBy = generatedBy,
            Parameters = parameters,
            Status = "queued",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void Complete(string reportUrl)
    {
        Status = "completed";
        ReportUrl = reportUrl;
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Fail()
    {
        Status = "failed";
        CompletedAt = DateTimeOffset.UtcNow;
    }
}
