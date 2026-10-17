namespace OpsFlow.Api.Contracts.Projects;

public sealed record CreateProjectRequest(
    Guid OrganizationId,
    string Name,
    string Key,
    string? Description);
