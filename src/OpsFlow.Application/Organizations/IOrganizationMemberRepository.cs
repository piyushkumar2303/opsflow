using OpsFlow.Domain.Entities;

namespace OpsFlow.Application.Organizations;

public interface IOrganizationMemberRepository
{
    Task<OrganizationMember?> GetByOrganizationAndUserAsync(
        Guid organizationId,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        OrganizationMember organizationMember,
        CancellationToken cancellationToken = default);
}
