using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobLocationConfiguration : IEntityTypeConfiguration<JobLocation>
{
    public void Configure(EntityTypeBuilder<JobLocation> builder)
    {
        builder.ToTable("job_locations");

        builder.HasKey(jobLocation => jobLocation.JobLocationId);

        builder.Property(jobLocation => jobLocation.JobLocationId).HasColumnName("job_location_id");
        builder.Property(jobLocation => jobLocation.JobId).HasColumnName("job_id");
        builder.Property(jobLocation => jobLocation.LocationId).HasColumnName("location_id");
        builder.Property(jobLocation => jobLocation.Latitude).HasColumnName("latitude").HasColumnType("numeric(10,7)");
        builder.Property(jobLocation => jobLocation.Longitude).HasColumnName("longitude").HasColumnType("numeric(10,7)");
        builder.Property(jobLocation => jobLocation.IsRemoteAllowed).HasColumnName("is_remote_allowed");

        // "geo_point" (PostGIS geography) intentionally left unmapped — see LocationConfiguration.

        builder.HasOne<Job>().WithMany().HasForeignKey(jobLocation => jobLocation.JobId);
        builder.HasOne<Location>().WithMany().HasForeignKey(jobLocation => jobLocation.LocationId);
    }
}
