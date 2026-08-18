using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RoomAvailabilityConfiguration : IEntityTypeConfiguration<RoomAvailability>
{
    public void Configure(EntityTypeBuilder<RoomAvailability> builder)
    {
        builder.ToTable("room_availability");

        builder.HasKey(availability => availability.RoomAvailabilityId);

        builder.Property(availability => availability.RoomAvailabilityId).HasColumnName("room_availability_id");
        builder.Property(availability => availability.RoomId).HasColumnName("room_id");
        builder.Property(availability => availability.AvailableFrom).HasColumnName("available_from");
        builder.Property(availability => availability.AvailableTo).HasColumnName("available_to");
        builder.Property(availability => availability.AvailableBeds).HasColumnName("available_beds");
        builder.Property(availability => availability.PriceOverride).HasColumnName("price_override").HasColumnType("numeric(12,2)");
        builder.Property(availability => availability.CreatedAt).HasColumnName("created_at");

        builder.HasOne<Room>().WithMany().HasForeignKey(availability => availability.RoomId);
    }
}
