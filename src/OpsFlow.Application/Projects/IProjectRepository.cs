using OpsFlow.Domain.Entities;
using OpsFlow.Application.Common.Models;
using OpsFlow.Application.Projects.ListProjects;

namespace OpsFlow.Application.Projects;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<bool> KeyExistsAsync(
        Guid organizationId,
        string key,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default);

    Task<PagedResult<ProjectListItem>> ListAsync(
        ListProjectsQuery query,
        CancellationToken cancellationToken = default);
}
