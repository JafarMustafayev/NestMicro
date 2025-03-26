namespace EventBus.RabbitMq.Abstractions;

public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    IModel CreateModel();
    bool TryConnect();
}