using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class AccommodationBookingConfiguration : IEntityTypeConfiguration<AccommodationBooking>
{
    public void Configure(EntityTypeBuilder<AccommodationBooking> builder)
    {
        builder.ToTable("accommodation_bookings");

        builder.HasKey(booking => booking.BookingId);

        builder.Property(booking => booking.BookingId).HasColumnName("booking_id");
        builder.Property(booking => booking.RoomId).HasColumnName("room_id");
        builder.Property(booking => booking.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(booking => booking.Status)
            .HasColumnName("status")
            .HasColumnType("booking_status");
        builder.Property(booking => booking.CheckInDate).HasColumnName("check_in_date");
        builder.Property(booking => booking.CheckOutDate).HasColumnName("check_out_date");
        builder.Property(booking => booking.TotalAmount).HasColumnName("total_amount").HasColumnType("numeric(12,2)");
        builder.Property(booking => booking.CreatedAt).HasColumnName("created_at");
        builder.Property(booking => booking.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<Room>().WithMany().HasForeignKey(booking => booking.RoomId);
        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(booking => booking.WorkerProfileId);
    }
}
