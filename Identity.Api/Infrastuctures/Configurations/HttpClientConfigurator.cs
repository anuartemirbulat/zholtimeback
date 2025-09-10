namespace Identity.Api.Infrastuctures.Configurations;

public static class HttpClientConfigurator
{
    public static void AddMobizonHttpClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("Mobizon",
            cfgClient =>
            {
                var cfg = configuration.GetSection("MobizonOptions");
                cfgClient.BaseAddress = new Uri(cfg.GetSection("Url").Value);
            });
    }
}
