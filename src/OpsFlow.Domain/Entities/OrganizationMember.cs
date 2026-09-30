using OpsFlow.Domain.Enums;

namespace OpsFlow.Domain.Entities;

public sealed class OrganizationMember
{
    public Guid Id { get; set; }

    public Guid OrganizationId { get; set; }

    public Guid UserId { get; set; }

    public OrganizationRole Role { get; set; }

    public DateTime JoinedAtUtc { get; set; }
}