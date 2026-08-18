using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("conversations");

        builder.HasKey(conversation => conversation.ConversationId);

        builder.Property(conversation => conversation.ConversationId).HasColumnName("conversation_id");
        builder.Property(conversation => conversation.ConversationType)
            .HasColumnName("conversation_type")
            .HasColumnType("conversation_type");
        builder.Property(conversation => conversation.Subject).HasColumnName("subject").HasMaxLength(180);
        builder.Property(conversation => conversation.RelatedEntityType).HasColumnName("related_entity_type").HasMaxLength(80);
        builder.Property(conversation => conversation.RelatedEntityId).HasColumnName("related_entity_id");
        builder.Property(conversation => conversation.CreatedBy).HasColumnName("created_by");
        builder.Property(conversation => conversation.CreatedAt).HasColumnName("created_at");
        builder.Property(conversation => conversation.LastMessageAt).HasColumnName("last_message_at");

        builder.HasOne<User>().WithMany().HasForeignKey(conversation => conversation.CreatedBy);

        builder.Navigation(conversation => conversation.Participants).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
