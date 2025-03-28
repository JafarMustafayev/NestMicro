namespace EventBus.RabbitMq.Extensions;

public static class DependencyInjection
{
    public static void AddRabbitMqEventBus(this IServiceCollection services)
    {
        //services.ConfigureRabbitMq();
        //services.AddSingletonServices();
        //services.AddTransientServices();
    }

    private static void ConfigureRabbitMq(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.86",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true
            };

            return new RabbitMqPersistentConnection(
                factory,
                sp.GetRequiredService<ILogger<RabbitMqPersistentConnection>>()
            );
        });
    }

    private static void AddSingletonServices(this IServiceCollection services)
    {
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, EventBusRabbitMq>();
    }

    private static void AddTransientServices(this IServiceCollection services)
    {
    }

    public static void UseRabbitMqEventBus(this IApplicationBuilder app)
    {
    }
}