using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Persistence.Configurations;

public sealed class OrganizationMemberConfiguration
    : IEntityTypeConfiguration<OrganizationMember>
{
    public void Configure(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.ToTable("OrganizationMembers");

        builder.HasKey(member => member.Id);

        builder.Property(member => member.Role)
            .IsRequired();

        builder.Property(member => member.JoinedAtUtc)
            .IsRequired();

        builder.HasIndex(member => new
        {
            member.OrganizationId,
            member.UserId
        })
        .IsUnique();

        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(member => member.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(member => member.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}