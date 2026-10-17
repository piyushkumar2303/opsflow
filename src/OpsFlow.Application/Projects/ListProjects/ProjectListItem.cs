using OpsFlow.Domain.Enums;

namespace OpsFlow.Application.Projects.ListProjects;

public sealed record ProjectListItem(
    Guid Id,
    string Name,
    string Key,
    string? Description,
    ProjectStatus Status,
    DateTime CreatedAtUtc);
