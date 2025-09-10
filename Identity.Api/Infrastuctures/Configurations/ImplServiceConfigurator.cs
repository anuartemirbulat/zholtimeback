using Identity.Services.Impl;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Services.Implementations;
using Services.Interfaces;

namespace Identity.Api.Infrastuctures.Configurations;

public static class ImplServiceConfigurator
{
    public static void AddImplServices(this IServiceCollection services, IConfiguration cfg, IHostEnvironment env)
    {
        if (env.IsProduction())
        {
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddScoped<IMessagingService, MobizonSmsService>();
        }
        else
        {
            services.AddTransient<IRegistrationService, MockRegistrationService>();
            services.AddScoped<IMessagingService, MockMobizonSmsService>();
        }

        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        services.AddTransient<IWaygoHttpService, WaygoHttpService>();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddRabbitMq(cfg);
        services.AddScoped<IdentityCurrentUserService>();
        services.AddScoped<IProfilesService, ProfilesService>();
    }
}