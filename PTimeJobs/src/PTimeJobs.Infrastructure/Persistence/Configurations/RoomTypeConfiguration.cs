using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
{
    public void Configure(EntityTypeBuilder<RoomType> builder)
    {
        builder.ToTable("room_types");

        builder.HasKey(roomType => roomType.RoomTypeId);

        builder.Property(roomType => roomType.RoomTypeId).HasColumnName("room_type_id");
        builder.Property(roomType => roomType.TypeName).HasColumnName("type_name").HasMaxLength(100);
        builder.Property(roomType => roomType.Description).HasColumnName("description");

        builder.HasIndex(roomType => roomType.TypeName).IsUnique();
    }
}
