using Microsoft.EntityFrameworkCore;
using OpsFlow.Domain.Entities;

namespace OpsFlow.Infrastructure.Persistence;

public sealed class OpsFlowDbContext : DbContext
{
    public OpsFlowDbContext(
        DbContextOptions<OpsFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Organization> Organizations => Set<Organization>();

    public DbSet<OrganizationMember> OrganizationMembers => Set<OrganizationMember>();

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<WorkTask> WorkTasks => Set<WorkTask>();

    public DbSet<Incident> Incidents => Set<Incident>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OpsFlowDbContext).Assembly);
    }
}
