using Common.Options;
using Microsoft.Extensions.Options;

namespace RegionalRides.Api.Infrastructure.Configurators;

public static class OptionsConfigurator
{
    public static void AddOptions(this IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
    {
        services.Configure<RabbitConfig>(opt => cfg.GetSection("RabbitConfig").Bind(opt));
        services.AddSingleton(x => x.GetService<IOptions<RabbitConfig>>().Value);
    }
}