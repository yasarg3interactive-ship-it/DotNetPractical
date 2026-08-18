using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Billing;
using PTimeJobs.Domain.Users;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoices");

        builder.HasKey(invoice => invoice.InvoiceId);

        builder.Property(invoice => invoice.InvoiceId).HasColumnName("invoice_id");
        builder.Property(invoice => invoice.UserId).HasColumnName("user_id");
        builder.Property(invoice => invoice.InvoiceNumber).HasColumnName("invoice_number").HasMaxLength(80);
        builder.Property(invoice => invoice.Currency).HasColumnName("currency").HasMaxLength(3).IsFixedLength();
        builder.Property(invoice => invoice.SubtotalAmount).HasColumnName("subtotal_amount").HasColumnType("numeric(12,2)");
        builder.Property(invoice => invoice.TaxAmount).HasColumnName("tax_amount").HasColumnType("numeric(12,2)");
        builder.Property(invoice => invoice.TotalAmount).HasColumnName("total_amount").HasColumnType("numeric(12,2)");
        builder.Property(invoice => invoice.Status)
            .HasColumnName("status")
            .HasColumnType("payment_status");
        builder.Property(invoice => invoice.IssuedAt).HasColumnName("issued_at");
        builder.Property(invoice => invoice.DueAt).HasColumnName("due_at");
        builder.Property(invoice => invoice.PaidAt).HasColumnName("paid_at");
        builder.Property(invoice => invoice.Metadata).HasColumnName("metadata").HasColumnType("jsonb");

        builder.HasIndex(invoice => invoice.InvoiceNumber).IsUnique();

        builder.HasOne<User>().WithMany().HasForeignKey(invoice => invoice.UserId);
    }
}
