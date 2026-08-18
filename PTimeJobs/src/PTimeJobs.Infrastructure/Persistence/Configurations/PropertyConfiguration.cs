using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("properties");

        builder.HasKey(property => property.PropertyId);

        builder.Property(property => property.PropertyId).HasColumnName("property_id");
        builder.Property(property => property.AccommodationProviderId).HasColumnName("accommodation_provider_id");
        builder.Property(property => property.PropertyName).HasColumnName("property_name").HasMaxLength(180);
        builder.Property(property => property.PropertyType).HasColumnName("property_type").HasMaxLength(80);
        builder.Property(property => property.Description).HasColumnName("description");
        builder.Property(property => property.LocationId).HasColumnName("location_id");
        builder.Property(property => property.Latitude).HasColumnName("latitude").HasColumnType("numeric(10,7)");
        builder.Property(property => property.Longitude).HasColumnName("longitude").HasColumnType("numeric(10,7)");
        builder.Property(property => property.AddressText).HasColumnName("address_text");
        builder.Property(property => property.IsActive).HasColumnName("is_active");
        builder.Property(property => property.AverageRating).HasColumnName("average_rating").HasColumnType("numeric(3,2)");
        builder.Property(property => property.RatingCount).HasColumnName("rating_count");
        builder.Property(property => property.CreatedAt).HasColumnName("created_at");
        builder.Property(property => property.UpdatedAt).HasColumnName("updated_at");

        // "geo_point" (PostGIS geography) intentionally left unmapped — see LocationConfiguration.

        builder.HasOne<AccommodationProvider>().WithMany().HasForeignKey(property => property.AccommodationProviderId);
        builder.HasOne<Location>().WithMany().HasForeignKey(property => property.LocationId);
    }
}
