namespace RegionalRides.Api.Infrastructure.Configurators;

public static class HttpClientConfigurator
{
    public static void AddIkHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("ic",
            cfgClient =>
            {
                var cfg = configuration.GetSection("Ik");
                cfgClient.BaseAddress = new Uri(cfg.GetSection("Url").Value);
            });
    }
}