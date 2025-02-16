namespace NestAuth.API;

public static class ServicesRegistrator
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue<string>("ServerIP")).Result;

        services.AddDbContext<AppDbContext>(options =>
        {
            var subsection = string.Empty;

            subsection = serverIsAvailable ? "SqlConnectionOnServer" : "SqlConnectionOnPrem";

            options.UseSqlServer(Configuration.GetConfiguratinValue<string>("ConnectionStrings", subsection));
        });

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

        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var subsection = serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem";
            consulConfig.Address = new Uri(Configuration.GetConfiguratinValue<string>("Consul", "ConsulServer", subsection)); //Consul server address
        }));
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }

    public static async void AddMassTransit(this IServiceCollection services)
    {
    }

    public static void RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var section = "Consul";

        var serverIsAvailable = InternetChecker.IsServerAvailable(Configuration.GetConfiguratinValue<string>("ServerIP")).Result;

        var route = Configuration.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck", "HealthCheckRoute");

        var consulClientHealthCheck = serverIsAvailable ?
            $"{Configuration.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck", "HealthCheckAddressOnServer")}{route}" :
            $"{Configuration.GetConfiguratinValue<string>(section, "ConsulClientHealthCheck", "HealthCheckAddressOnPrem")}{route}";

        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var registration = new AgentServiceRegistration()
        {
            ID = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerId"),
            Name = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerName"),
            Address = Configuration.GetConfiguratinValue<string>(section, "ConsulClientRegister", "ServerAddress"),
            Port = Configuration.GetConfiguratinValue<int>(section, "ConsulClientRegister", "ServerPort"),
            Tags = new[] { "NestAuth", "Auth", "Identity", "Server" },
            Check = new AgentServiceCheck()
            {
                HTTP = consulClientHealthCheck, // health check address
                Timeout = TimeSpan.FromSeconds(5),
                Interval = TimeSpan.FromSeconds(10),
                DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                TLSSkipVerify = true
            }
        };

        try
        {
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            Console.WriteLine($"Service registered with Consul. Health check URL: {registration.Check.HTTP}");

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering with Consul: {ex.Message}");
        }
    }
}