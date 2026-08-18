using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Employers;
using PTimeJobs.Domain.Jobs;
using PTimeJobs.Domain.Workers;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.ToTable("contracts");

        builder.HasKey(contract => contract.ContractId);

        builder.Property(contract => contract.ContractId).HasColumnName("contract_id");
        builder.Property(contract => contract.JobId).HasColumnName("job_id");
        builder.Property(contract => contract.ApplicationId).HasColumnName("application_id");
        builder.Property(contract => contract.WorkerProfileId).HasColumnName("worker_profile_id");
        builder.Property(contract => contract.EmployerProfileId).HasColumnName("employer_profile_id");
        builder.Property(contract => contract.Status)
            .HasColumnName("status")
            .HasColumnType("contract_status");
        builder.Property(contract => contract.StartDate).HasColumnName("start_date");
        builder.Property(contract => contract.EndDate).HasColumnName("end_date");
        builder.Property(contract => contract.AgreedSalary).HasColumnName("agreed_salary").HasColumnType("numeric(12,2)");
        builder.Property(contract => contract.SalaryModel)
            .HasColumnName("salary_model")
            .HasColumnType("salary_model");
        builder.Property(contract => contract.TermsUrl).HasColumnName("terms_url");
        builder.Property(contract => contract.CreatedAt).HasColumnName("created_at");
        builder.Property(contract => contract.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(contract => contract.ApplicationId).IsUnique();

        builder.HasOne<Job>().WithMany().HasForeignKey(contract => contract.JobId);
        builder.HasOne<JobApplication>().WithMany().HasForeignKey(contract => contract.ApplicationId);
        builder.HasOne<WorkerProfile>().WithMany().HasForeignKey(contract => contract.WorkerProfileId);
        builder.HasOne<EmployerProfile>().WithMany().HasForeignKey(contract => contract.EmployerProfileId);
    }
}
