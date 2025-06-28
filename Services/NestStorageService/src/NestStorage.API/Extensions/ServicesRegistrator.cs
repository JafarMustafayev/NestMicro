namespace NestStorage.API.Extensions;

public static class ServicesRegistrator
{
    public static void AddStorageServices(this IServiceCollection services)
    {
        services.ConnectSqlServer();

        services.AddConsul();

        services.AddFluent();

        services.AddRepository();

        services.AddServices();

        services.AddEventsHandlers();

        //services.AddAutoMapper(typeof(GetEmailTemplateDto).Assembly);

        services.ConfigureRabbitMq();
    }

    private static void ConnectSqlServer(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => { options.UseSqlServer(Configurations.GetConnectionString("DefaultConnection")); });
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
        //services.AddValidatorsFromAssembly(typeof(CreateEmailTemplateValidator).Assembly);
    }

    private static void AddRepository(this IServiceCollection services)
    {
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus>(sp =>
            new EventBusRabbitMq(
                sp.GetRequiredService<IRabbitMqPersistentConnection>(),
                sp.GetRequiredService<IEventBusSubscriptionsManager>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                "Storage"
            ));
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

    public static void UseRabbitMqEventBus(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
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
}