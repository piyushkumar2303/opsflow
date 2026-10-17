using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Organizations;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;

namespace OpsFlow.Infrastructure.Repositories;

public sealed class OrganizationMemberRepository
    : IOrganizationMemberRepository
{
    private readonly OpsFlowDbContext _dbContext;

    public OrganizationMemberRepository(
        OpsFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrganizationMember?>
        GetByOrganizationAndUserAsync(
            Guid organizationId,
            Guid userId,
            CancellationToken cancellationToken = default)
    {
        return await _dbContext.OrganizationMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                member =>
                    member.OrganizationId == organizationId &&
                    member.UserId == userId,
                cancellationToken);
    }

    public async Task AddAsync(
        OrganizationMember organizationMember,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.OrganizationMembers.AddAsync(
            organizationMember,
            cancellationToken);
    }
}
