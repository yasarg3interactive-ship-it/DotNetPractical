using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ShortlistConfiguration : IEntityTypeConfiguration<Shortlist>
{
    public void Configure(EntityTypeBuilder<Shortlist> builder)
    {
        builder.ToTable("shortlists");

        builder.HasKey(shortlist => shortlist.ShortlistId);

        builder.Property(shortlist => shortlist.ShortlistId).HasColumnName("shortlist_id");
        builder.Property(shortlist => shortlist.ApplicationId).HasColumnName("application_id");
        builder.Property(shortlist => shortlist.ShortlistedBy).HasColumnName("shortlisted_by");
        builder.Property(shortlist => shortlist.Notes).HasColumnName("notes");
        builder.Property(shortlist => shortlist.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(shortlist => shortlist.ApplicationId).IsUnique();

        builder.HasOne<JobApplication>().WithOne().HasForeignKey<Shortlist>(shortlist => shortlist.ApplicationId);
        builder.HasOne<User>().WithMany().HasForeignKey(shortlist => shortlist.ShortlistedBy);
    }
}
