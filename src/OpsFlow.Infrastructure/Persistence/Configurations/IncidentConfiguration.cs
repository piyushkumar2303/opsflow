using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Persistence.Configurations;

public sealed class IncidentConfiguration
    : IEntityTypeConfiguration<Incident>
{
    public void Configure(EntityTypeBuilder<Incident> builder)
    {
        builder.ToTable("Incidents");

        builder.HasKey(incident => incident.Id);

        builder.Property(incident => incident.SequenceNumber)
            .IsRequired();

        builder.Property(incident => incident.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(incident => incident.Description)
            .HasMaxLength(4000);

        builder.Property(incident => incident.Status)
            .IsRequired();

        builder.Property(incident => incident.Severity)
            .IsRequired();

        builder.Property(incident => incident.CreatedAtUtc)
            .IsRequired();

        builder.HasIndex(incident => new
        {
            incident.ProjectId,
            incident.SequenceNumber
        })
        .IsUnique();

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(incident => incident.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(incident => incident.ReportedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(incident => incident.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
