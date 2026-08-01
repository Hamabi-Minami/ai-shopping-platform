using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyShippingPlatform.Application.Common.Interfaces;
using MyShippingPlatform.Infrastructure.Persistence;
using MyShippingPlatform.Infrastructure.Services;

namespace MyShippingPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<ApplicationDbContext>());

        // Register the AI Agent service with its typed HttpClient
        services.AddHttpClient<IAiAgentService, OllamaAgentService>();

        return services;
    }
}