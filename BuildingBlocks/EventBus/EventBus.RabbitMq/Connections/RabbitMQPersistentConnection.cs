namespace EventBus.RabbitMq.Connections;

public class RabbitMqPersistentConnection : IRabbitMqPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMqPersistentConnection> _logger;
    private IConnection _connection;
    private bool _disposed;

    public RabbitMqPersistentConnection(
        IConnectionFactory connectionFactory,
        ILogger<RabbitMqPersistentConnection> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public bool IsConnected => _connection?.IsOpen == true && !_disposed;

    public IModel CreateModel()
    {
        if (IsConnected)
        {
            return _connection.CreateModel();
        }

        throw new InvalidOperationException("RabbitMQ is not connected");
    }

    public bool TryConnect()
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            _connection.ConnectionShutdown += (s, e) =>
            {
                if (_disposed) return;
                _logger.LogWarning("RabbitMQ connection lost. Trying to reconnect...");
                TryConnect();
            };

            _logger.LogInformation("RabbitMQ connection successfully established");
            return true;
        }
        catch (BrokerUnreachableException)
        {
            Thread.Sleep(2000);
            return TryConnect();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _connection?.Dispose();
    }
}