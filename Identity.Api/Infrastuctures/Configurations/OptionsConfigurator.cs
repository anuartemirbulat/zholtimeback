using System.Configuration;
using Common.Options;
using Microsoft.Extensions.Options;

namespace Identity.Api.Infrastuctures.Configurations;

public static class OptionsConfigurator
{
    public static void AddOptions(this IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
    {
        services.Configure<MobizonOptions>(opt => cfg.GetSection("MobizonOptions").Bind(opt));
        services.AddSingleton(x => x.GetService<IOptions<MobizonOptions>>().Value);
        
        services.Configure<RabbitConfig>(opt => cfg.GetSection("RabbitConfig").Bind(opt));
        services.AddSingleton(x => x.GetService<IOptions<RabbitConfig>>().Value);
    }
}
