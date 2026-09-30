using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Persistence.Configurations;

public sealed class OrganizationConfiguration
    : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(organization => organization.Id);

        builder.Property(organization => organization.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(organization => organization.CreatedAtUtc)
            .IsRequired();
    }
}