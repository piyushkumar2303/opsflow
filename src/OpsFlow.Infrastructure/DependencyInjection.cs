using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpsFlow.Infrastructure.Persistence;
using OpsFlow.Application.Projects;
using OpsFlow.Infrastructure.Repositories;
using OpsFlow.Application.Common.Interfaces;
using OpsFlow.Infrastructure.Security;
using OpsFlow.Application.Users;

namespace OpsFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<OpsFlowDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork>(serviceProvider =>
            serviceProvider.GetRequiredService<OpsFlowDbContext>());

        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPasswordHasher, AspNetCorePasswordHasher>();

        return services;
    }
}
