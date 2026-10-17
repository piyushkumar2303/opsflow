using OpsFlow.Domain.Entities;
using OpsFlow.Application.Organizations.ListMyOrganizations;

namespace OpsFlow.Application.Organizations;

public interface IOrganizationRepository
{
    Task<bool> ExistsAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MyOrganizationItem>> ListOrganizationsForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
