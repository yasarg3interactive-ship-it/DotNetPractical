using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Billing;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.HasKey(transaction => transaction.TransactionId);

        builder.Property(transaction => transaction.TransactionId).HasColumnName("transaction_id");
        builder.Property(transaction => transaction.PaymentId).HasColumnName("payment_id");
        builder.Property(transaction => transaction.TransactionType).HasColumnName("transaction_type").HasMaxLength(80);
        builder.Property(transaction => transaction.Amount).HasColumnName("amount").HasColumnType("numeric(12,2)");
        builder.Property(transaction => transaction.Status)
            .HasColumnName("status")
            .HasColumnType("payment_status");
        builder.Property(transaction => transaction.ProviderTransactionId).HasColumnName("provider_transaction_id").HasMaxLength(160);
        builder.Property(transaction => transaction.ProviderResponse).HasColumnName("provider_response").HasColumnType("jsonb");
        builder.Property(transaction => transaction.CreatedAt).HasColumnName("created_at");

        builder.HasOne<Payment>().WithMany().HasForeignKey(transaction => transaction.PaymentId);
    }
}
