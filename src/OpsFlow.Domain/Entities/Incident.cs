using OpsFlow.Domain.Enums;

namespace OpsFlow.Domain.Entities;

public sealed class Incident
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public int SequenceNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public IncidentStatus Status { get; set; }

    public IncidentSeverity Severity { get; set; }

    public Guid? AssignedToUserId { get; set; }

    public Guid ReportedByUserId { get; set; }

    public DateTime? ResolvedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
