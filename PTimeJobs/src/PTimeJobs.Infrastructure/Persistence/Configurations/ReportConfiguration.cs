using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Reports;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable("reports");

        builder.HasKey(report => report.ReportId);

        builder.Property(report => report.ReportId).HasColumnName("report_id");
        builder.Property(report => report.ReportType).HasColumnName("report_type").HasMaxLength(100);
        builder.Property(report => report.GeneratedBy).HasColumnName("generated_by");
        builder.Property(report => report.Parameters).HasColumnName("parameters").HasColumnType("jsonb");
        builder.Property(report => report.ReportUrl).HasColumnName("report_url");
        builder.Property(report => report.Status).HasColumnName("status").HasMaxLength(40);
        builder.Property(report => report.CreatedAt).HasColumnName("created_at");
        builder.Property(report => report.CompletedAt).HasColumnName("completed_at");

        builder.HasOne<User>().WithMany().HasForeignKey(report => report.GeneratedBy);
    }
}
