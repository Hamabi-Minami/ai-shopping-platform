using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyShippingPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Automatically register all MediatR Handlers in this assembly
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // Note: We can add MediatR Pipeline Behaviors here later (e.g., for FluentValidation)
        });

        return services;
    }
}