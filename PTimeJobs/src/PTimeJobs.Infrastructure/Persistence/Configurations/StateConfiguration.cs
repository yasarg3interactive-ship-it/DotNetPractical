using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PTimeJobs.Domain.Locations;

namespace PTimeJobs.Infrastructure.Persistence.Configurations;

public sealed class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("states");

        builder.HasKey(state => state.StateId);

        builder.Property(state => state.StateId).HasColumnName("state_id");
        builder.Property(state => state.CountryId).HasColumnName("country_id");
        builder.Property(state => state.StateName).HasColumnName("state_name").HasMaxLength(140);
        builder.Property(state => state.StateCode).HasColumnName("state_code").HasMaxLength(30);
        builder.Property(state => state.CreatedAt).HasColumnName("created_at");

        builder.HasIndex(state => new { state.CountryId, state.StateName }).IsUnique();

        builder.HasOne<Country>()
            .WithMany(country => country.States)
            .HasForeignKey(state => state.CountryId);

        builder.Navigation(state => state.Cities).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
