namespace NestAuth.API.Extensions;

public static class ServicesRegistrator
{
    public static void AddAuthServices(this IServiceCollection services)
    {
        services.ConnectSqlServer();

        services.AddIdentity();

        services.ConfigureRabbitMq();

        services.AddConsul();

        services.AddFluent();

        services.AddRedis();

        services.AddRepository();

        services.AddServices();

        services.AddEventsHandlers();
    }

    private static void ConnectSqlServer(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(Configurations.GetConnectionString("DefaultConnection")); });
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
                options.SignIn.RequireConfirmedAccount = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
        {
            var messageBroker = Configurations.GetConfiguration<MessageBroker>();

            var config = Configurations.IsProduction() ? messageBroker.RabbitMQ.Production : messageBroker.RabbitMQ.Development;

            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                Port = config.Port,
                UserName = config.UserName,
                Password = config.Password,
                DispatchConsumersAsync = config.DispatchConsumersAsync,
                AutomaticRecoveryEnabled = config.AutomaticRecoveryEnabled
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

    private static void AddConsul(this IServiceCollection services)
    {
        var consul = Configurations.GetConfiguration<ServiceDiscovery>();
        var address = consul.Consul.Endpoints;

        services.AddSingleton<IConsulClient, ConsulClient>(p => new(consulConfig =>
        {
            consulConfig.Address = new(
                Configurations.IsProduction() ? address.Production : address.Production); //Consul server address
        }));
    }

    private static void AddFluent(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
    }

    private static void AddRedis(this IServiceCollection services)
    {
        var redisCaching = Configurations.GetConfiguration<Caching>().Redis;

        var redisConfig = Configurations.IsProduction() ? redisCaching.Production : redisCaching.Development;
        services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(redisConfig.ConnectionString)
        );
    }

    private static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUserSessionRepository, UserSessionRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserManagementService, UserManagementService>();
        services.AddScoped<IPasswordManagementService, PasswordManagementService>();
        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ICurrentUserDataService, CurrentUserDataService>();
        services.AddScoped<IUserSessionService, UserSessionService>();
        services.AddScoped<IUserDeviceInfoService, UserDeviceInfoService>();
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus>(sp =>
            new EventBusRabbitMq(
                sp.GetRequiredService<IRabbitMqPersistentConnection>(),
                sp.GetRequiredService<IEventBusSubscriptionsManager>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                "Auth" // Suffix əl ilə ötürülür
            ));
        services.AddSingleton<ICacheService, CacheService>();
    }

    public static async Task RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime)
    {
        var serviceDiscovery = Configurations.GetConfiguration<ServiceDiscovery>().Consul;
        var endpoint = Configurations.IsProduction() ? serviceDiscovery.HealthCheck.Endpoints.Production : serviceDiscovery.HealthCheck.Endpoints.Production;

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