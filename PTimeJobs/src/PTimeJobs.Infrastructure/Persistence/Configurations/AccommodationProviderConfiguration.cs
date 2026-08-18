using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Accommodation;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class AccommodationProviderConfiguration : IEntityTypeConfiguration<AccommodationProvider>
{
    public void Configure(EntityTypeBuilder<AccommodationProvider> builder)
    {
        builder.ToTable("accommodation_providers");

        builder.HasKey(provider => provider.AccommodationProviderId);

        builder.Property(provider => provider.AccommodationProviderId).HasColumnName("accommodation_provider_id");
        builder.Property(provider => provider.UserId).HasColumnName("user_id");
        builder.Property(provider => provider.BusinessName).HasColumnName("business_name").HasMaxLength(180);
        builder.Property(provider => provider.VerificationStatus)
            .HasColumnName("verification_status")
            .HasColumnType("verification_status");
        builder.Property(provider => provider.ContactNumber).HasColumnName("contact_number").HasMaxLength(20);
        builder.Property(provider => provider.CreatedAt).HasColumnName("created_at");
        builder.Property(provider => provider.UpdatedAt).HasColumnName("updated_at");

        builder.HasOne<User>().WithMany().HasForeignKey(provider => provider.UserId);
    }
}
