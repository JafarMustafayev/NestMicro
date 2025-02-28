namespace NestAPIGateway.Extensions;

public static class ServicesRegistrator
{
    public static void AddServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker.IsServerAvailable(Configurations.GetConfiguratinValue<string>("ServerIP")).Result;

        AddConsul(services, serverIsAvailable);
    }

    private static void AddConsul(IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configurations.GetConfiguratinValue<string>
                ("Consul", "ConsulServer",
                serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem")); //Consul server address
        }));
    }

    public static async Task RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var section = "Consul";

        var serverIsAvailable = InternetChecker.IsServerAvailable(Configurations.GetConfiguratinValue<string>("ServerIP")).Result;

        var route = Configurations.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck", "HealthCheckRoute");

        var consulClientHealthCheck =
            $"{Configurations.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck",
            (serverIsAvailable ? "HealthCheckAddressOnServer" : "HealthCheckAddressOnPrem"))}{route}";

        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration()
        {
            ID = Configurations.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerId"),
            Name = Configurations.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerName"),
            Address = Configurations.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerAddress"),
            Port = Configurations.GetConfiguratinValue<int>(section, "ConsulClientRegister", "ServerPort"),
            Tags = new[] { "Api Gateway" },
            Check = new AgentServiceCheck()
            {
                HTTP = consulClientHealthCheck, // health check address
                Timeout = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(5),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                TLSSkipVerify = true
            },
            EnableTagOverride = true
        };
        try
        {
            await consulClient.Agent.ServiceDeregister(registration.ID);
            await consulClient.Agent.ServiceRegister(registration);

            lifetime.ApplicationStopping.Register(async () =>
            {
                await consulClient.Agent.ServiceDeregister(registration.ID);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($" -------\n\nError registering with Consul: {ex.Message}\n\n -------");
        }
    }
}