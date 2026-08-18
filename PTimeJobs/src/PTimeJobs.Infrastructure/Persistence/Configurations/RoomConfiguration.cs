using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("rooms");

        builder.HasKey(room => room.RoomId);

        builder.Property(room => room.RoomId).HasColumnName("room_id");
        builder.Property(room => room.PropertyId).HasColumnName("property_id");
        builder.Property(room => room.RoomTypeId).HasColumnName("room_type_id");
        builder.Property(room => room.RoomNumber).HasColumnName("room_number").HasMaxLength(80);
        builder.Property(room => room.FloorNumber).HasColumnName("floor_number").HasMaxLength(40);
        builder.Property(room => room.Capacity).HasColumnName("capacity");
        builder.Property(room => room.OccupiedCount).HasColumnName("occupied_count");
        builder.Property(room => room.MonthlyPrice).HasColumnName("monthly_price").HasColumnType("numeric(12,2)");
        builder.Property(room => room.SecurityDeposit).HasColumnName("security_deposit").HasColumnType("numeric(12,2)");
        builder.Property(room => room.IsAvailable).HasColumnName("is_available");
        builder.Property(room => room.CreatedAt).HasColumnName("created_at");
        builder.Property(room => room.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<Property>().WithMany().HasForeignKey(room => room.PropertyId);
        builder.HasOne<RoomType>().WithMany().HasForeignKey(room => room.RoomTypeId);
    }
}
