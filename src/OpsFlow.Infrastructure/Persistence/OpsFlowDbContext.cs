using Microsoft.EntityFrameworkCore;

namespace OpsFlow.Infrastructure.Persistence;

public sealed class OpsFlowDbContext : DbContext
{
    public OpsFlowDbContext(
        DbContextOptions<OpsFlowDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OpsFlowDbContext).Assembly);
    }
}
