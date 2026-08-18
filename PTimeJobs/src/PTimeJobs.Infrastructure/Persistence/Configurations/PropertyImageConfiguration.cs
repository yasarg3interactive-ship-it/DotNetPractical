using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class PropertyImageConfiguration : IEntityTypeConfiguration<PropertyImage>
{
    public void Configure(EntityTypeBuilder<PropertyImage> builder)
    {
        builder.ToTable("property_images");

        builder.HasKey(image => image.PropertyImageId);

        builder.Property(image => image.PropertyImageId).HasColumnName("property_image_id");
        builder.Property(image => image.PropertyId).HasColumnName("property_id");
        builder.Property(image => image.ImageUrl).HasColumnName("image_url");
        builder.Property(image => image.SortOrder).HasColumnName("sort_order");
        builder.Property(image => image.IsPrimary).HasColumnName("is_primary");
        builder.Property(image => image.CreatedAt).HasColumnName("created_at");

        builder.HasOne<Property>().WithMany().HasForeignKey(image => image.PropertyId);
    }
}
