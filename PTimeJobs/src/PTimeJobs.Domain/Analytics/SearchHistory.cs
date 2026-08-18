namespace PTimeJobs.Domain.Analytics;

public sealed class SearchHistory
{
    private SearchHistory()
    {
    }

    public Guid SearchId { get; private set; }
    public Guid? UserId { get; private set; }
    public string SearchScope { get; private set; } = string.Empty;
    public string? QueryText { get; private set; }
    public string Filters { get; private set; } = "{}";
    public int? ResultCount { get; private set; }
    public Guid? LocationId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static SearchHistory Create(
        string searchScope,
        Guid? userId = null,
        string? queryText = null,
        int? resultCount = null,
        Guid? locationId = null)
    {
        if (string.IsNullOrWhiteSpace(searchScope))
        {
            throw new InvalidOperationException("Search scope is required.");
        }

        return new SearchHistory
        {
            SearchId = Guid.NewGuid(),
            UserId = userId,
            SearchScope = searchScope,
            QueryText = queryText,
            ResultCount = resultCount,
            LocationId = locationId,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
