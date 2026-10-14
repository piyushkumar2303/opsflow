using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Organizations;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;

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
}
