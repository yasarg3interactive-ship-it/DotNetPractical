using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Messaging;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class MessageAttachmentConfiguration : IEntityTypeConfiguration<MessageAttachment>
{
    public void Configure(EntityTypeBuilder<MessageAttachment> builder)
    {
        builder.ToTable("message_attachments");

        builder.HasKey(attachment => attachment.MessageAttachmentId);

        builder.Property(attachment => attachment.MessageAttachmentId).HasColumnName("message_attachment_id");
        builder.Property(attachment => attachment.MessageId).HasColumnName("message_id");
        builder.Property(attachment => attachment.FileUrl).HasColumnName("file_url");
        builder.Property(attachment => attachment.FileName).HasColumnName("file_name").HasMaxLength(240);
        builder.Property(attachment => attachment.MimeType).HasColumnName("mime_type").HasMaxLength(120);
        builder.Property(attachment => attachment.FileSizeBytes).HasColumnName("file_size_bytes");
        builder.Property(attachment => attachment.CreatedAt).HasColumnName("created_at");

        builder.HasOne<Message>().WithMany().HasForeignKey(attachment => attachment.MessageId);
    }
}
