namespace NestAPIGateway.Extensions;

public static class ServicesRegistrator
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddConsul();
    }

    private static void AddConsul(this IServiceCollection services)
    {
        var consul = Configurations.GetConfiguration<ServiceDiscovery>();
        var address = consul.Consul.Endpoints;

        services.AddSingleton<IConsulClient, ConsulClient>(p => new(consulConfig =>
        {
            consulConfig.Address = new(
                Configurations.IsProduction() ? address.Production : address.Development); //Consul server address
        }));
    }

    public static async Task RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var serviceDiscovery = Configurations.GetConfiguration<ServiceDiscovery>().Consul;
        var endpoint = Configurations.IsProduction() ? serviceDiscovery.HealthCheck.Endpoints.Production : serviceDiscovery.HealthCheck.Endpoints.Development;

        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration
        {
            ID = serviceDiscovery.ServiceRegistration.ServiceId,
            Name = serviceDiscovery.ServiceRegistration.ServiceName,
            Address = serviceDiscovery.ServiceRegistration.Address,
            Port = serviceDiscovery.ServiceRegistration.Port,
            Tags = serviceDiscovery.ServiceRegistration.Tags,
            Check = new()
            {
                HTTP = $"{endpoint}{serviceDiscovery.HealthCheck.HealthCheckPath}", // health check address
                Timeout = TimeSpan.FromSeconds(serviceDiscovery.HealthCheck.Timeout),
                Interval = TimeSpan.FromSeconds(serviceDiscovery.HealthCheck.Interval),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(serviceDiscovery.HealthCheck.DeregisterCriticalServiceAfter),
                TLSSkipVerify = serviceDiscovery.HealthCheck.TLSSkipVerify
            },
            EnableTagOverride = serviceDiscovery.ServiceRegistration.EnableTagOverride
        };
        try
        {
            await consulClient.Agent.ServiceDeregister(registration.ID);
            await consulClient.Agent.ServiceRegister(registration);

            lifetime.ApplicationStopping.Register(async () => { await consulClient.Agent.ServiceDeregister(registration.ID); });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering with Consul: {ex.Message}");
        }
    }
}