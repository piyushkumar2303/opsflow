using OpsFlow.Domain.Enums;

namespace OpsFlow.Domain.Entities;

public sealed class WorkTask
{
    public Guid Id { get; set; }

    public Guid ProjectId { get; set; }

    public int SequenceNumber { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public WorkTaskStatus Status { get; set; }

    public WorkTaskPriority Priority { get; set; }

    public Guid? AssignedToUserId { get; set; }

    public Guid CreatedByUserId { get; set; }

    public DateTime? DueAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }
}
