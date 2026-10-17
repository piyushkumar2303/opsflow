using OpsFlow.Domain.Enums;

namespace OpsFlow.Application.Projects.GetProjectById;

public sealed record GetProjectByIdResult(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Key,
    string? Description,
    ProjectStatus Status,
    Guid CreatedByUserId,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
