using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("countries");

        builder.HasKey(country => country.CountryId);

        builder.Property(country => country.CountryId).HasColumnName("country_id");
        builder.Property(country => country.Iso2).HasColumnName("iso2").HasMaxLength(2).IsFixedLength();
        builder.Property(country => country.Iso3).HasColumnName("iso3").HasMaxLength(3).IsFixedLength();
        builder.Property(country => country.CountryName).HasColumnName("country_name").HasMaxLength(120);
        builder.Property(country => country.PhoneCode).HasColumnName("phone_code").HasMaxLength(10);
        builder.Property(country => country.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(country => country.Iso2).IsUnique();
        builder.HasIndex(country => country.Iso3).IsUnique();

        builder.Navigation(country => country.States).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
