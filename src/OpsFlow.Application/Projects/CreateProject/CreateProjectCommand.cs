namespace OpsFlow.Application.Projects.CreateProject;

public sealed record CreateProjectCommand(
    Guid OrganizationId,
    string Name,
    string Key,
    string? Description);
