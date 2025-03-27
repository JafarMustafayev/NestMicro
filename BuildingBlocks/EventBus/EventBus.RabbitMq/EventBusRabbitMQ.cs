namespace EventBus.RabbitMq;

public class EventBusRabbitMq : IEventBus, IDisposable
{
    private readonly IRabbitMqPersistentConnection _persistentConnection;
    private readonly IEventBusSubscriptionsManager _subsManager;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IModel _consumerChannel;
    private const string ExchangeName = "event_bus";

    public EventBusRabbitMq(
        IRabbitMqPersistentConnection persistentConnection,
        IEventBusSubscriptionsManager subsManager,
        IServiceScopeFactory serviceScopeFactory)
    {
        _persistentConnection = persistentConnection;
        _subsManager = subsManager;
        _serviceScopeFactory = serviceScopeFactory;
        InitializeRabbitMQ();
    }

    private void InitializeRabbitMQ()
    {
        _consumerChannel = CreateConsumerChannel();
        _subsManager.OnEventRemoved += OnEventRemoved;
    }

    public Task PublishAsync(IntegrationEvent @event)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var eventName = @event.GetType().Name;
        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());

        using var channel = _persistentConnection.CreateModel();
        var properties = channel.CreateBasicProperties();
        properties.DeliveryMode = 2; // Persistent

        channel.BasicPublish(
            exchange: ExchangeName,
            routingKey: eventName,
            mandatory: true,
            basicProperties: properties,
            body: body
        );

        return Task.CompletedTask;
    }

    public void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        _subsManager.AddSubscription<T, TH>();

        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        // Kanalı USING-dən çıxarın! 
        var channel = _persistentConnection.CreateModel();
        var queueName = GetQueueName(eventName);

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        channel.QueueBind(
            queue: queueName,
            exchange: ExchangeName,
            routingKey: eventName
        );

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var message = Encoding.UTF8.GetString(ea.Body.Span);
            await ProcessEvent(ea.RoutingKey, message);
            channel.BasicAck(ea.DeliveryTag, multiple: false);
        };

        channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumer
        );
    }

    public void Unsubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
    {
        _subsManager.RemoveSubscription<T, TH>();
    }

    private IModel CreateConsumerChannel()
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        var channel = _persistentConnection.CreateModel();
        channel.ExchangeDeclare(
            exchange: ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );
        return channel;
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        using var scope = _serviceScopeFactory.CreateScope();

        if (!_subsManager.HasSubscriptionsForEvent(eventName))
            return;

        var eventType = _subsManager.GetEventTypeByName(eventName);

        var @event = JsonSerializer.Deserialize(message, eventType) as IntegrationEvent;
        var handlers = _subsManager.GetHandlersForEvent(eventName);

        foreach (var handlerType in handlers)
        {
            var handler = scope.ServiceProvider.GetService(handlerType);
            if (handler == null) continue;

            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
        }
    }

    private void OnEventRemoved(object sender, string eventName)
    {
        if (!_persistentConnection.IsConnected)
        {
            _persistentConnection.TryConnect();
        }

        using var channel = _persistentConnection.CreateModel();
        channel.QueueUnbind(
            queue: GetQueueName(eventName),
            exchange: ExchangeName,
            routingKey: eventName
        );
    }

    private static string GetQueueName(string eventName) => $"{ExchangeName}_{eventName}_queue";

    public void Dispose()
    {
        _consumerChannel?.Dispose();
        _subsManager.Clear();
    }
}