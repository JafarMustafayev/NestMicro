namespace EventBus.RabbitMq.Abstractions;

public interface IRabbitMqPersistentConnection : IDisposable
{
    bool IsConnected { get; }
    IModel CreateModel();
    bool TryConnect();
}