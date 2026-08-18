using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class DeliveryAreaConfiguration : IEntityTypeConfiguration<DeliveryArea>
{
    public void Configure(EntityTypeBuilder<DeliveryArea> builder)
    {
        builder.ToTable("delivery_areas");

        builder.HasKey(deliveryArea => deliveryArea.DeliveryAreaId);

        builder.Property(deliveryArea => deliveryArea.DeliveryAreaId).HasColumnName("delivery_area_id");
        builder.Property(deliveryArea => deliveryArea.FoodProviderId).HasColumnName("food_provider_id");
        builder.Property(deliveryArea => deliveryArea.AreaId).HasColumnName("area_id");
        builder.Property(deliveryArea => deliveryArea.RadiusKm).HasColumnName("radius_km").HasColumnType("numeric(6,2)");
        builder.Property(deliveryArea => deliveryArea.DeliveryFee).HasColumnName("delivery_fee").HasColumnType("numeric(12,2)");
        builder.Property(deliveryArea => deliveryArea.IsActive).HasColumnName("is_active");

        builder.HasOne<FoodProvider>().WithMany().HasForeignKey(deliveryArea => deliveryArea.FoodProviderId);
        builder.HasOne<Area>().WithMany().HasForeignKey(deliveryArea => deliveryArea.AreaId);
    }
}
