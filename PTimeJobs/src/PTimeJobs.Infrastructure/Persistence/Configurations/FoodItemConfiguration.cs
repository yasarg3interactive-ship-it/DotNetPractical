using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.ToTable("food_items");

        builder.HasKey(item => item.FoodItemId);

        builder.Property(item => item.FoodItemId).HasColumnName("food_item_id");
        builder.Property(item => item.FoodProviderId).HasColumnName("food_provider_id");
        builder.Property(item => item.ItemName).HasColumnName("item_name").HasMaxLength(160);
        builder.Property(item => item.Description).HasColumnName("description");
        builder.Property(item => item.FoodType).HasColumnName("food_type").HasMaxLength(60);
        builder.Property(item => item.Price).HasColumnName("price").HasColumnType("numeric(12,2)");
        builder.Property(item => item.IsAvailable).HasColumnName("is_available");
        builder.Property(item => item.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(item => item.CreatedAt).HasColumnName("created_at");

        builder.HasOne<FoodProvider>().WithMany().HasForeignKey(item => item.FoodProviderId);
    }
}
