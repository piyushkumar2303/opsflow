using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Organizations;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;
using OpsFlow.Application.Organizations.ListMyOrganizations;

namespace OpsFlow.Infrastructure.Repositories;

public sealed class OrganizationRepository
    : IOrganizationRepository
{
    private readonly OpsFlowDbContext _dbContext;

    public OrganizationRepository(
        OpsFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistsAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Organizations
            .AnyAsync(
                organization => organization.Id == organizationId,
                cancellationToken);
    }

    public async Task AddAsync(
        Organization organization,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Organizations.AddAsync(
            organization,
            cancellationToken);
    }

    public async Task<IReadOnlyList<MyOrganizationItem>> ListOrganizationsForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await (
            from member in _dbContext.OrganizationMembers.AsNoTracking()
            join organization in _dbContext.Organizations.AsNoTracking()
                on member.OrganizationId equals organization.Id
            where member.UserId == userId
            orderby organization.Name, organization.Id
            select new MyOrganizationItem(
                organization.Id,
                organization.Name,
                member.Role)
        ).ToListAsync(cancellationToken);
    }
}
