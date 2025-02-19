namespace NestAuth.API;

public static class ServicesRegistrator
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue<string>("ServerIP")).Result;

        ConnectSqlServer(services, serverIsAvailable);

        AddIdentity(services);

        AddConsul(services, serverIsAvailable);

        AddServices(services);

        AddMassTransit(services);
    }

    private static void ConnectSqlServer(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var subsection = string.Empty;

            options.UseSqlServer(Configuration.GetConfiguratinValue<string>("ConnectionStrings",
                (serverIsAvailable ? "SqlConnectionOnServer" : "SqlConnectionOnPrem")));
        });
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddConsul(IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configuration.GetConfiguratinValue<string>
                ("Consul", "ConsulServer",
                (serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem"))); //Consul server address
        }));
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }

    private static async void AddMassTransit(this IServiceCollection services)
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