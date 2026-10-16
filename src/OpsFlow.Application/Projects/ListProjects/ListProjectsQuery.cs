using OpsFlow.Domain.Enums;

namespace OpsFlow.Application.Projects.ListProjects;

public sealed record ListProjectsQuery(
    Guid OrganizationId,
    int PageNumber = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    ProjectStatus? Status = null);
