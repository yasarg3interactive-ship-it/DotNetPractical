namespace PTimeJobs.Domain.Food;

public sealed class FoodItem
{
    private FoodItem()
    {
    }

    public Guid FoodItemId { get; private set; }
    public Guid FoodProviderId { get; private set; }
    public string ItemName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? FoodType { get; private set; }
    public decimal Price { get; private set; }
    public bool IsAvailable { get; private set; }
    public string Metadata { get; private set; } = "{}";
    public DateTimeOffset CreatedAt { get; private set; }

    public static FoodItem Create(
        Guid foodProviderId,
        string itemName,
        decimal price,
        string? description = null,
        string? foodType = null)
    {
        if (string.IsNullOrWhiteSpace(itemName))
        {
            throw new InvalidOperationException("Item name is required.");
        }

        if (price < 0)
        {
            throw new InvalidOperationException("Price cannot be negative.");
        }

        return new FoodItem
        {
            FoodItemId = Guid.NewGuid(),
            FoodProviderId = foodProviderId,
            ItemName = itemName,
            Description = description,
            FoodType = foodType,
            Price = price,
            IsAvailable = true,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    public void MarkUnavailable()
    {
        IsAvailable = false;
    }

    public void MarkAvailable()
    {
        IsAvailable = true;
    }
}
