namespace PTimeJobs.Application.Food.Dtos;

public sealed record FoodProviderResponse(
    Guid FoodProviderId,
    Guid UserId,
    string BusinessName,
    string ProviderType,
    string VerificationStatus,
    Guid? LocationId,
    decimal AverageRating,
    int RatingCount,
    DateTimeOffset CreatedAt);

public sealed record CreateFoodProviderRequest(Guid UserId, string BusinessName, string ProviderType, Guid? LocationId);

public sealed record DeliveryAreaResponse(Guid DeliveryAreaId, Guid FoodProviderId, Guid? AreaId, decimal? RadiusKm, decimal DeliveryFee, bool IsActive);

public sealed record CreateDeliveryAreaRequest(Guid? AreaId, decimal? RadiusKm, decimal DeliveryFee);

public sealed record FoodItemResponse(
    Guid FoodItemId,
    Guid FoodProviderId,
    string ItemName,
    string? Description,
    string? FoodType,
    decimal Price,
    bool IsAvailable);

public sealed record CreateFoodItemRequest(Guid FoodProviderId, string ItemName, decimal Price, string? Description, string? FoodType);

public sealed record FoodPlanItemResponse(Guid FoodPlanId, Guid FoodItemId, string ItemName, string MealSlot);

public sealed record AddFoodPlanItemRequest(Guid FoodItemId, string MealSlot);

public sealed record FoodPlanResponse(
    Guid FoodPlanId,
    Guid FoodProviderId,
    string PlanName,
    string? Description,
    int DurationDays,
    decimal Price,
    int MealsPerDay,
    bool IsActive,
    IReadOnlyCollection<FoodPlanItemResponse> Items);

public sealed record CreateFoodPlanRequest(
    Guid FoodProviderId,
    string PlanName,
    int DurationDays,
    decimal Price,
    int MealsPerDay,
    string? Description);

public sealed record FoodSubscriptionResponse(
    Guid FoodSubscriptionId,
    Guid FoodPlanId,
    Guid UserId,
    string Status,
    DateOnly StartDate,
    DateOnly? EndDate,
    Guid? DeliveryLocationId);

public sealed record CreateFoodSubscriptionRequest(Guid FoodPlanId, Guid UserId, DateOnly StartDate, Guid? DeliveryLocationId);
