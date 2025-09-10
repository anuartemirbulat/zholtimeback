using System.ComponentModel.Design;
using RegionalRides.Services.Implementations;
using RegionalRides.Services.Interfaces;
using Services.Implementations;
using Services.Interfaces;

namespace RegionalRides.Api.Infrastructure.Configurators;

public static class ServicesConfigurator
{
    public static void AddImplServices(this IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
    {
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddTransient<IWaygoHttpService, WaygoHttpService>();
        services.AddScoped<IReferencesService, ReferencesService>();
        services.AddScoped<RegRidesCurrentUserService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddRabbitMq(cfg);
    }
}
