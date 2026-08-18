using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Messaging;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ConversationParticipantConfiguration : IEntityTypeConfiguration<ConversationParticipant>
{
    public void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        builder.ToTable("conversation_participants");

        builder.HasKey(participant => new { participant.ConversationId, participant.UserId });

        builder.Property(participant => participant.ConversationId).HasColumnName("conversation_id");
        builder.Property(participant => participant.UserId).HasColumnName("user_id");
        builder.Property(participant => participant.JoinedAt).HasColumnName("joined_at");
        builder.Property(participant => participant.LastReadAt).HasColumnName("last_read_at");
        builder.Property(participant => participant.IsMuted).HasColumnName("is_muted");

        builder.HasOne<Conversation>()
            .WithMany(conversation => conversation.Participants)
            .HasForeignKey(participant => participant.ConversationId);

        builder.HasOne<User>().WithMany().HasForeignKey(participant => participant.UserId);
    }
}
