namespace PTimeJobs.Domain.Food;

public sealed class DeliveryArea
{
    private DeliveryArea()
    {
    }

    public Guid DeliveryAreaId { get; private set; }
    public Guid FoodProviderId { get; private set; }
    public Guid? AreaId { get; private set; }
    public decimal? RadiusKm { get; private set; }
    public decimal DeliveryFee { get; private set; }
    public bool IsActive { get; private set; }

    public static DeliveryArea Create(Guid foodProviderId, Guid? areaId = null, decimal? radiusKm = null, decimal deliveryFee = 0m)
    {
        return new DeliveryArea
        {
            DeliveryAreaId = Guid.NewGuid(),
            FoodProviderId = foodProviderId,
            AreaId = areaId,
            RadiusKm = radiusKm,
            DeliveryFee = deliveryFee,
            IsActive = true
        };
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
