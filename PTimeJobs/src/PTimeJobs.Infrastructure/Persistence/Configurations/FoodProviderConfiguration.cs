using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Food;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FoodProviderConfiguration : IEntityTypeConfiguration<FoodProvider>
{
    public void Configure(EntityTypeBuilder<FoodProvider> builder)
    {
        builder.ToTable("food_providers");

        builder.HasKey(provider => provider.FoodProviderId);

        builder.Property(provider => provider.FoodProviderId).HasColumnName("food_provider_id");
        builder.Property(provider => provider.UserId).HasColumnName("user_id");
        builder.Property(provider => provider.BusinessName).HasColumnName("business_name").HasMaxLength(180);
        builder.Property(provider => provider.ProviderType).HasColumnName("provider_type").HasMaxLength(80);
        builder.Property(provider => provider.VerificationStatus)
            .HasColumnName("verification_status")
            .HasColumnType("verification_status");
        builder.Property(provider => provider.LocationId).HasColumnName("location_id");
        builder.Property(provider => provider.AverageRating).HasColumnName("average_rating").HasColumnType("numeric(3,2)");
        builder.Property(provider => provider.RatingCount).HasColumnName("rating_count");
        builder.Property(provider => provider.CreatedAt).HasColumnName("created_at");
        builder.Property(provider => provider.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<User>().WithMany().HasForeignKey(provider => provider.UserId);
        builder.HasOne<Location>().WithMany().HasForeignKey(provider => provider.LocationId);
    }
}
