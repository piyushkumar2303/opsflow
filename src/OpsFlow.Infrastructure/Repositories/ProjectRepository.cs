using Microsoft.EntityFrameworkCore;
using OpsFlow.Application.Projects;
using OpsFlow.Domain.Entities;
using OpsFlow.Infrastructure.Persistence;
using OpsFlow.Application.Common.Models;
using OpsFlow.Application.Projects.ListProjects;

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

    public async Task<PagedResult<ProjectListItem>> ListAsync(
        ListProjectsQuery query,
        CancellationToken cancellationToken = default)
    {
        var projects = _dbContext.Projects
            .AsNoTracking()
            .Where(project => project.OrganizationId == query.OrganizationId);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim();

            projects = projects.Where(project =>
                project.Name.Contains(searchTerm) ||
                project.Key.Contains(searchTerm));
        }

        if (query.Status.HasValue)
        {
            projects = projects.Where(project =>
                project.Status == query.Status.Value);
        }

        var totalCount = await projects.CountAsync(cancellationToken);

        var items = await projects
            .OrderByDescending(project => project.CreatedAtUtc)
            .ThenByDescending(project => project.Id)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(project => new ProjectListItem(
                project.Id,
                project.Name,
                project.Key,
                project.Description,
                project.Status,
                project.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return new PagedResult<ProjectListItem>(
            items,
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}
