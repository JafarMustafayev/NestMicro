namespace NestAuth.API.Extensions;

public static class ServicesRegistrator
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker
            .IsServerAvailable(Configurations.GetConfiguratinValue<string>("ServerIP")).Result;

        services.ConnectSqlServer(serverIsAvailable);

        services.AddIdentity();

        services.ConfigureRabbitMq(serverIsAvailable);

        services.AddConsul(serverIsAvailable);

        services.AddFluent();

        services.AddRepository();

        services.AddServices();

        services.AddEventsHandlers();
    }

    private static void ConnectSqlServer(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var subsection = string.Empty;

            options.UseSqlServer(Configurations.GetConfiguratinValue<string>("ConnectionStrings",
                (serverIsAvailable ? "SqlConnectionOnServer" : "SqlConnectionOnPrem")));
        });
    }

    private static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureRabbitMq(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
        {
            var subSection = (serverIsAvailable ? "DockerHostOnServer" : "DockerHostOnPrem");

            var factory = new ConnectionFactory()
            {
                HostName = Configurations.GetConfiguratinValue<string>("RabbitMq", subSection, "HostName"),
                UserName = Configurations.GetConfiguratinValue<string>("RabbitMq", subSection, "UserName"),
                Password = Configurations.GetConfiguratinValue<string>("RabbitMq", subSection, "Password"),
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };

            return new RabbitMqPersistentConnection(
                factory,
                sp.GetRequiredService<ILogger<RabbitMqPersistentConnection>>()
            );
        });
    }

    private static void AddEventsHandlers(this IServiceCollection services)
    {
    }

    private static void AddConsul(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configurations.GetConfiguratinValue<string>("Consul",
                "ConsulServer",
                (serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem"))); //Consul server address
        }));
    }

    private static void AddFluent(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
    }

    private static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IUserDeviceInfoService, UserDeviceInfoService>();
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, EventBusRabbitMq>();
    }

    public static async Task RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var section = "Consul";

        var serverIsAvailable = InternetChecker
            .IsServerAvailable(Configurations.GetConfiguratinValue<string>("ServerIP")).Result;

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
            Console.WriteLine($"Error registering with Consul: {ex.Message}");
        }
    }

    public static async Task SeedingDB(this IServiceScope scope)
    {
        using (scope)
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                await DbInitializer.SeedAsync(serviceProvider);
            }
            catch (Exception ex)
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}