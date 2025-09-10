using Common.Options;
using MassTransit;
using RegionalRides.Api.Consumers;

namespace RegionalRides.Api.Infrastructure.Configurators;

public static class MasstransitConfigurator
{
    public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
    {
        var sp = services.BuildServiceProvider();
        services.AddMassTransit(x =>
        {
            var rabbitConfig = sp.GetService<RabbitConfig>();
            x.AddConsumer<NewProfileConsumer>();
            x.AddConsumer<DeleteProfileConsumer>();
            var rabbitHost = new Uri(new Uri(rabbitConfig.HostName), rabbitConfig.VirtualHostName);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitHost, h =>
                {
                    //default admin user = myuser mypassword
                    h.Username(rabbitConfig.UserName);
                    h.Password(rabbitConfig.Password);
                });
                cfg.ReceiveEndpoint("regional_queue", ep =>
                {
                    ep.PrefetchCount = 32;
                    ep.ConfigureConsumer<NewProfileConsumer>(context);
                    ep.ConfigureConsumer<DeleteProfileConsumer>(context);
                });
            });
        });
    }
}