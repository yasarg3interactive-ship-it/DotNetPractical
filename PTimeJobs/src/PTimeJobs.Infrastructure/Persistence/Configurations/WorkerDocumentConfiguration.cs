using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Users;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class WorkerDocumentConfiguration : IEntityTypeConfiguration<WorkerDocument>
{
    public void Configure(EntityTypeBuilder<WorkerDocument> builder)
    {
        builder.ToTable("worker_documents");

        builder.HasKey(document => document.DocumentId);

        builder.Property(document => document.DocumentId).HasColumnName("document_id");
        builder.Property(document => document.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(document => document.DocumentType).HasColumnName("document_type").HasMaxLength(80);
        builder.Property(document => document.DocumentUrl).HasColumnName("document_url");
        builder.Property(document => document.FileName).HasColumnName("file_name").HasMaxLength(240);
        builder.Property(document => document.MimeType).HasColumnName("mime_type").HasMaxLength(120);
        builder.Property(document => document.VerificationStatus)
            .HasColumnName("verification_status")
            .HasColumnType("verification_status");
        builder.Property(document => document.VerifiedBy).HasColumnName("verified_by");
        builder.Property(document => document.VerifiedAt).HasColumnName("verified_at");
        builder.Property(document => document.CreatedAt).HasColumnName("created_at");

        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(document => document.WorkerProfileId);
        builder.HasOne<User>().WithMany().HasForeignKey(document => document.VerifiedBy);
    }
}
