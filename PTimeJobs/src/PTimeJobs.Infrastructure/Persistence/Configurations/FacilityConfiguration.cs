using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable("facilities");

        builder.HasKey(facility => facility.FacilityId);

        builder.Property(facility => facility.FacilityId).HasColumnName("facility_id");
        builder.Property(facility => facility.FacilityName).HasColumnName("facility_name").HasMaxLength(120);
        builder.Property(facility => facility.FacilityCategory).HasColumnName("facility_category").HasMaxLength(80);

        builder.HasIndex(facility => facility.FacilityName).IsUnique();
    }
}
