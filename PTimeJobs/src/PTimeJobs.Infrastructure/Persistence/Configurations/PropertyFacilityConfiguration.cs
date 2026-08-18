using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class PropertyFacilityConfiguration : IEntityTypeConfiguration<PropertyFacility>
{
    public void Configure(EntityTypeBuilder<PropertyFacility> builder)
    {
        builder.ToTable("property_facilities");

        builder.HasKey(propertyFacility => new { propertyFacility.PropertyId, propertyFacility.FacilityId });

        builder.Property(propertyFacility => propertyFacility.PropertyId).HasColumnName("property_id");
        builder.Property(propertyFacility => propertyFacility.FacilityId).HasColumnName("facility_id");
        builder.Property(propertyFacility => propertyFacility.Details).HasColumnName("details");

        builder.HasOne<Property>().WithMany().HasForeignKey(propertyFacility => propertyFacility.PropertyId);
        builder.HasOne<Facility>().WithMany().HasForeignKey(propertyFacility => propertyFacility.FacilityId);
    }
}
