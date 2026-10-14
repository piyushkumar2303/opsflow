using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OpsFlow.Application.Projects.CreateProject;
using OpsFlow.Application.Users.RegisterUser;
using OpsFlow.Application.Users.LoginUser;
using OpsFlow.Application.Organizations.CreateOrganization;

namespace OpsFlow.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();

        services.AddScoped<CreateProjectHandler>();

        services.AddScoped<CreateOrganizationHandler>();

        services.AddScoped<RegisterUserHandler>();

        services.AddScoped<LoginUserHandler>();

        return services;
    }
}
