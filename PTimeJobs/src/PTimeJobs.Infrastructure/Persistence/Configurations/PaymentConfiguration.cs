using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(payment => payment.PaymentId);

        builder.Property(payment => payment.PaymentId).HasColumnName("payment_id");
        builder.Property(payment => payment.UserId).HasColumnName("user_id");
        builder.Property(payment => payment.InvoiceId).HasColumnName("invoice_id");
        builder.Property(payment => payment.PayableEntityType).HasColumnName("payable_entity_type").HasMaxLength(80);
        builder.Property(payment => payment.PayableEntityId).HasColumnName("payable_entity_id");
        builder.Property(payment => payment.Currency).HasColumnName("currency").HasMaxLength(3).IsFixedLength();
        builder.Property(payment => payment.Amount).HasColumnName("amount").HasColumnType("numeric(12,2)");
        builder.Property(payment => payment.Status)
            .HasColumnName("status")
            .HasColumnType("payment_status");
        builder.Property(payment => payment.PaymentMethod).HasColumnName("payment_method").HasMaxLength(80);
        builder.Property(payment => payment.ProviderName).HasColumnName("provider_name").HasMaxLength(80);
        builder.Property(payment => payment.ProviderPaymentId).HasColumnName("provider_payment_id").HasMaxLength(160);
        builder.Property(payment => payment.CreatedAt).HasColumnName("created_at");
        builder.Property(payment => payment.PaidAt).HasColumnName("paid_at");
        builder.Property(payment => payment.Metadata).HasColumnName("metadata").HasColumnType("jsonb");

        builder.HasOne<User>().WithMany().HasForeignKey(payment => payment.UserId);
        builder.HasOne<Invoice>().WithMany().HasForeignKey(payment => payment.InvoiceId);
    }
}
