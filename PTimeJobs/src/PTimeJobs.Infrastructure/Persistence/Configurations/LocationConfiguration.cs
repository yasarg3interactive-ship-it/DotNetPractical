using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("locations");

        builder.HasKey(location => location.LocationId);

        builder.Property(location => location.LocationId).HasColumnName("location_id");
        builder.Property(location => location.CountryId).HasColumnName("country_id");
        builder.Property(location => location.StateId).HasColumnName("state_id");
        builder.Property(location => location.CityId).HasColumnName("city_id");
        builder.Property(location => location.AreaId).HasColumnName("area_id");
        builder.Property(location => location.AddressLine1).HasColumnName("address_line1");
        builder.Property(location => location.AddressLine2).HasColumnName("address_line2");
        builder.Property(location => location.Landmark).HasColumnName("landmark");
        builder.Property(location => location.Latitude).HasColumnName("latitude").HasColumnType("numeric(10,7)");
        builder.Property(location => location.Longitude).HasColumnName("longitude").HasColumnType("numeric(10,7)");
        builder.Property(location => location.GooglePlaceId).HasColumnName("google_place_id").HasMaxLength(160);
        builder.Property(location => location.CreatedAt).HasColumnName("created_at");

        // "geo_point" (PostGIS geography, derived from latitude/longitude) is intentionally
        // left unmapped here — reading/writing it would need the NetTopologySuite plugin.
        // It's nullable in the DB, so EF simply leaves it untouched.

        builder.HasOne<Country>().WithMany().HasForeignKey(location => location.CountryId);
        builder.HasOne<State>().WithMany().HasForeignKey(location => location.StateId);
        builder.HasOne<City>().WithMany().HasForeignKey(location => location.CityId);
        builder.HasOne<Area>().WithMany().HasForeignKey(location => location.AreaId);
    }
}
