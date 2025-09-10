using Common.Options;
using MassTransit;

namespace Identity.Api.Infrastuctures.Configurations;

public static class MasstransitConfigurator
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var sp = services.BuildServiceProvider();
        services.AddMassTransit(x =>
        {
            var rabbitConfig = sp.GetService<RabbitConfig>();
            var rabbitHost = new Uri(new Uri(rabbitConfig.HostName), rabbitConfig.VirtualHostName);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost, h =>
                {
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });
                cfg.ReceiveEndpoint("identity_queue", ep => { ep.PrefetchCount = 32; });
            });
        });
    }
}