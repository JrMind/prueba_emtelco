using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<IExternalCharacterService, ExternalCharacterService>();
        services.AddSingleton<ILoggerService, FileLoggerService>(); 
        return services;
    }
}
