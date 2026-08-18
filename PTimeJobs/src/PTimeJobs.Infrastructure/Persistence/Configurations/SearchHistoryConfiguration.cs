using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Analytics;
using PTimeJobs.Domain.Locations;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class SearchHistoryConfiguration : IEntityTypeConfiguration<SearchHistory>
{
    public void Configure(EntityTypeBuilder<SearchHistory> builder)
    {
        builder.ToTable("search_history");

        builder.HasKey(search => search.SearchId);

        builder.Property(search => search.SearchId).HasColumnName("search_id");
        builder.Property(search => search.UserId).HasColumnName("user_id");
        builder.Property(search => search.SearchScope).HasColumnName("search_scope").HasMaxLength(80);
        builder.Property(search => search.QueryText).HasColumnName("query_text");
        builder.Property(search => search.Filters).HasColumnName("filters").HasColumnType("jsonb");
        builder.Property(search => search.ResultCount).HasColumnName("result_count");
        builder.Property(search => search.LocationId).HasColumnName("location_id");
        builder.Property(search => search.CreatedAt).HasColumnName("created_at");

        builder.HasOne<User>().WithMany().HasForeignKey(search => search.UserId);
        builder.HasOne<Location>().WithMany().HasForeignKey(search => search.LocationId);
    }
}
