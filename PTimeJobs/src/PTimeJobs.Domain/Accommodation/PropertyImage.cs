namespace PTimeJobs.Domain.Accommodation;

public sealed class PropertyImage
{
    private PropertyImage()
    {
    }

    public Guid PropertyImageId { get; private set; }
    public Guid PropertyId { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public int SortOrder { get; private set; }
    public bool IsPrimary { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    public static PropertyImage Create(Guid propertyId, string imageUrl, int sortOrder = 0, bool isPrimary = false)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            throw new InvalidOperationException("Image URL is required.");
        }

        return new PropertyImage
        {
            PropertyImageId = Guid.NewGuid(),
            PropertyId = propertyId,
            ImageUrl = imageUrl,
            SortOrder = sortOrder,
            IsPrimary = isPrimary,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
