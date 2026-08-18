using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Jobs;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class JobCategoryConfiguration : IEntityTypeConfiguration<JobCategory>
{
    public void Configure(EntityTypeBuilder<JobCategory> builder)
    {
        builder.ToTable("job_categories");

        builder.HasKey(category => category.JobCategoryId);

        builder.Property(category => category.JobCategoryId).HasColumnName("job_category_id");
        builder.Property(category => category.ParentCategoryId).HasColumnName("parent_category_id");
        builder.Property(category => category.CategoryName).HasColumnName("category_name").HasMaxLength(140);
        builder.Property(category => category.CategorySlug).HasColumnName("category_slug").HasMaxLength(160);
        builder.Property(category => category.IsActive).HasColumnName("is_active");

        builder.HasIndex(category => category.CategorySlug).IsUnique();

        builder.HasOne<JobCategory>().WithMany().HasForeignKey(category => category.ParentCategoryId);
    }
}
