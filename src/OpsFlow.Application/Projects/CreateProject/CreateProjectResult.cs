namespace OpsFlow.Application.Projects.CreateProject;

public sealed record CreateProjectResult(
    Guid Id,
    Guid OrganizationId,
    string Name,
    string Key,
    string? Description);
