using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(city => city.CityId);

        builder.Property(city => city.CityId).HasColumnName("city_id");
        builder.Property(city => city.StateId).HasColumnName("state_id");
        builder.Property(city => city.CityName).HasColumnName("city_name").HasMaxLength(140);
        builder.Property(city => city.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(city => new { city.StateId, city.CityName }).IsUnique();

        builder.HasOne<State>()
            .WithMany(state => state.Cities)
            .HasForeignKey(city => city.StateId);

        builder.Navigation(city => city.Areas).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
