using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OpsFlow.Application.Projects.CreateProject;
using OpsFlow.Application.Users.RegisterUser;

namespace OpsFlow.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateProjectCommandValidator>();

        services.AddScoped<CreateProjectHandler>();
        
        services.AddScoped<RegisterUserHandler>();


        return services;
    }
}
