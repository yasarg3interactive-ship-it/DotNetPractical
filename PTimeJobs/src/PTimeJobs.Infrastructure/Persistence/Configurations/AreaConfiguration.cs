using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.ToTable("areas");

        builder.HasKey(area => area.AreaId);

        builder.Property(area => area.AreaId).HasColumnName("area_id");
        builder.Property(area => area.CityId).HasColumnName("city_id");
        builder.Property(area => area.AreaName).HasColumnName("area_name").HasMaxLength(160);
        builder.Property(area => area.PostalCode).HasColumnName("postal_code").HasMaxLength(20);
        builder.Property(area => area.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(area => new { area.CityId, area.AreaName, area.PostalCode }).IsUnique();

        builder.HasOne<City>()
            .WithMany(city => city.Areas)
            .HasForeignKey(area => area.CityId);
    }
}
