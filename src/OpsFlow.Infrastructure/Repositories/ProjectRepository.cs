using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Projects;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;

namespace OpsFlow.Infrastructure.Repositories;

public sealed class ProjectRepository : IProjectRepository
{
    private readonly OpsFlowDbContext _dbContext;

    public ProjectRepository(OpsFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Project?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                project => project.Id == id,
                cancellationToken);
    }

    public async Task<bool> KeyExistsAsync(
        Guid organizationId,
        string key,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Projects
            .AnyAsync(
                project =>
                    project.OrganizationId == organizationId &&
                    project.Key == key,
                cancellationToken);
    }

    public async Task AddAsync(
        Project project,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Projects.AddAsync(
            project,
            cancellationToken);
    }
}
