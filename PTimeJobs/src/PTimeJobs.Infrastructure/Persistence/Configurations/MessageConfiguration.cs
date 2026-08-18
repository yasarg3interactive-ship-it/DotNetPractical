using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("messages");

        builder.HasKey(message => message.MessageId);

        builder.Property(message => message.MessageId).HasColumnName("message_id");
        builder.Property(message => message.ConversationId).HasColumnName("conversation_id");
        builder.Property(message => message.SenderUserId).HasColumnName("sender_user_id");
        builder.Property(message => message.Body).HasColumnName("body");
        builder.Property(message => message.Metadata).HasColumnName("metadata").HasColumnType("jsonb");
        builder.Property(message => message.SentAt).HasColumnName("sent_at");
        builder.Property(message => message.EditedAt).HasColumnName("edited_at");
        builder.Property(message => message.DeletedAt).HasColumnName("deleted_at");

        builder.HasOne<Conversation>().WithMany().HasForeignKey(message => message.ConversationId);
        builder.HasOne<User>().WithMany().HasForeignKey(message => message.SenderUserId);
    }
}
