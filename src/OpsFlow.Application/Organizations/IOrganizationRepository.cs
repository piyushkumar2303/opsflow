using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Organizations;

public interface IOrganizationRepository
{
    Task<bool> ExistsAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken = default);
}
