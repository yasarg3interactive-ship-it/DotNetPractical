using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Complaints;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.ToTable("complaints");

        builder.HasKey(complaint => complaint.ComplaintId);

        builder.Property(complaint => complaint.ComplaintId).HasColumnName("complaint_id");
        builder.Property(complaint => complaint.ComplainantUserId).HasColumnName("complainant_user_id");
        builder.Property(complaint => complaint.TargetEntityType).HasColumnName("target_entity_type").HasMaxLength(80);
        builder.Property(complaint => complaint.TargetEntityId).HasColumnName("target_entity_id");
        builder.Property(complaint => complaint.ComplaintCategory).HasColumnName("complaint_category").HasMaxLength(100);
        builder.Property(complaint => complaint.Description).HasColumnName("description");
        builder.Property(complaint => complaint.Status).HasColumnName("status").HasColumnType("complaint_status");
        builder.Property(complaint => complaint.AssignedTo).HasColumnName("assigned_to");
        builder.Property(complaint => complaint.ResolutionNotes).HasColumnName("resolution_notes");
        builder.Property(complaint => complaint.CreatedAt).HasColumnName("created_at");
        builder.Property(complaint => complaint.ResolvedAt).HasColumnName("resolved_at");

        builder.HasOne<User>().WithMany().HasForeignKey(complaint => complaint.ComplainantUserId);
        builder.HasOne<User>().WithMany().HasForeignKey(complaint => complaint.AssignedTo);
    }
}
