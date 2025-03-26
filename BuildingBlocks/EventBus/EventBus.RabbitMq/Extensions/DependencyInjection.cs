namespace EventBus.RabbitMq.Extensions;

public static class DependencyInjection
{
    public static void AddRabbitMqEventBus(this IServiceCollection services)
    {
        services.AddTransient<IIntegrationEventHandler<TestEvent>, TestEventHandler>();

        services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.86",
                UserName = "guest",
                Password = "guest",
                Port = 5672,
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true, // Əhəmiyyətli!
                RequestedHeartbeat = TimeSpan.FromSeconds(60)
            };

            Console.WriteLine($"Creating a new RabbitMQ connection: {factory.HostName}");

            return new RabbitMQPersistentConnection(factory,
                sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>());
        });
        services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        services.AddSingleton<IEventBus, EventBusRabbitMQ>();
    }
}