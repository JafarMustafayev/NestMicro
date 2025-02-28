namespace NestNotification.API;

public static class ServicesRegistrator
{
    public static void AddNotificationServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue<string>("ServerIP")).Result;

        services.ConnectSqlServer(serverIsAvailable);

        services.AddConsul(serverIsAvailable);

        //services.AddFluent();

        services.AddRepository();

        services.AddServices();

        services.AddMassTransit();
    }

    private static void ConnectSqlServer(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var subsection = string.Empty;

            options.UseSqlServer(Configuration.GetConfiguratinValue<string>("ConnectionStrings",
                serverIsAvailable ? "SqlConnectionOnServer" : "SqlConnectionOnPrem"));
        });
    }

    private static void AddConsul(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configuration.GetConfiguratinValue<string>
                ("Consul", "ConsulServer",
                serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem")); //Consul server address
        }));
    }

    //private static void AddFluent(this IServiceCollection services)
    //{
    //    services.AddFluentValidationAutoValidation();
    //    services.AddFluentValidationClientsideAdapters();
    //    services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
    //}

    private static void AddRepository(this IServiceCollection services)
    {
    }

    private static void AddServices(this IServiceCollection services)
    {
    }

    private static void AddMassTransit(this IServiceCollection services)
    {
    }

    public static async Task RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var section = "Consul";

        var serverIsAvailable = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue<string>("ServerIP")).Result;

        var route = Configuration.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck", "HealthCheckRoute");

        var consulClientHealthCheck =
            $"{Configuration.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck",
            (serverIsAvailable ? "HealthCheckAddressOnServer" : "HealthCheckAddressOnPrem"))}{route}";

        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration()
        {
            ID = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerId"),
            Name = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerName"),
            Address = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerAddress"),
            Port = Configuration.GetConfiguratinValue<int>(section, "ConsulClientRegister", "ServerPort"),
            Tags = new[] { "NestA0uth", "Auth", "Identity", "Server" },
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