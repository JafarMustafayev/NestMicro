namespace NestNotification.API.Extensions;

public static class ServicesRegistrator
{
    public static void AddNotificationServices(this IServiceCollection services)
    {
        var serverIsAvailable = InternetChecker
            .IsServerAvailable(Configurations.GetConfiguratinValue<string>("ServerIP")).Result;

        services.ConnectSqlServer(serverIsAvailable);

        services.AddConsul(serverIsAvailable);

        services.AddFluent();

        services.AddRepository();

        services.AddServices();

        services.AddEventsHandlers();

        services.AddAutoMapper(typeof(GetEmailTemplateDto).Assembly);

        services.ConfigureRabbitMq(serverIsAvailable);
    }

    private static void ConnectSqlServer(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(Configurations.GetConfiguratinValue<string>("ConnectionStrings",
                serverIsAvailable ? "SqlConnectionOnServer" : "SqlConnectionOnPrem"));
        });
    }

    private static void AddConsul(this IServiceCollection services, bool serverIsAvailable)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            consulConfig.Address = new Uri(Configurations.GetConfiguratinValue<string>("Consul",
                "ConsulServer",
                serverIsAvailable ? "ConsulServerOnServer" : "ConsulServerOnPrem")); //Consul server address
        }));
    }

    private static void AddFluent(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(typeof(CreateEmailTemplateValidator).Assembly);
    }

    private static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IEmailLogRepository, EmailLogRepository>();
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
        services.AddScoped<IEmailQueueRepository, EmailQueueRepository>();
        services.AddScoped<IEmailTemplateAttributeRepository, EmailTemplateAttributeRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus>(sp =>
            new EventBusRabbitMq(
                sp.GetRequiredService<IRabbitMqPersistentConnection>(),
                sp.GetRequiredService<IEventBusSubscriptionsManager>(),
                sp.GetRequiredService<IServiceScopeFactory>(),
                "notification"
            ));
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
        services.AddTransient<NewUserLoginDetectedIntegrationEventHandler>();
        services.AddTransient<UserRegisteredIntegrationEventHandler>();
        services.AddTransient<UserEmailConfirmedIntegrationEventHandler>();
        services.AddTransient<UserPasswordResetRequestedIntegrationEventHandler>();

        services.AddTransient<IIntegrationEventHandler<NewUserLoginDetectedIntegrationEvent>, NewUserLoginDetectedIntegrationEventHandler>();
        services.AddTransient<IIntegrationEventHandler<UserRegisteredIntegrationEvent>, UserRegisteredIntegrationEventHandler>();
        services.AddTransient<IIntegrationEventHandler<UserEmailConfirmedIntegrationEvent>, UserEmailConfirmedIntegrationEventHandler>();
        services.AddTransient<IIntegrationEventHandler<UserPasswordResetRequestedIntegrationEvent>, UserPasswordResetRequestedIntegrationEventHandler>();
    }

    public static void UseRabbitMqEventBus(this IApplicationBuilder app)
    {
        var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

        eventBus.Subscribe<NewUserLoginDetectedIntegrationEvent, NewUserLoginDetectedIntegrationEventHandler>();
        eventBus.Subscribe<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>();
        eventBus.Subscribe<UserEmailConfirmedIntegrationEvent, UserEmailConfirmedIntegrationEventHandler>();
        eventBus.Subscribe<UserPasswordResetRequestedIntegrationEvent, UserPasswordResetRequestedIntegrationEventHandler>();
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

            lifetime.ApplicationStopping.Register(async () => { await consulClient.Agent.ServiceDeregister(registration.ID); });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering with Consul: {ex.Message}");
        }
    }
}