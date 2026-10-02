using OpsFlow.Domain.Entities;

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
}
