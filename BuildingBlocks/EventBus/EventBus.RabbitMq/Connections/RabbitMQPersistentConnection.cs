namespace EventBus.RabbitMq.Connections;

public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly ILogger<RabbitMQPersistentConnection> _logger;
    private IConnection _connection;
    private bool _disposed;

    public RabbitMQPersistentConnection(
        IConnectionFactory connectionFactory,
        ILogger<RabbitMQPersistentConnection> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public bool IsConnected => _connection?.IsOpen == true && !_disposed;

    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            throw new InvalidOperationException("No RabbitMQ connections available");
        }

        return _connection.CreateModel();
    }

    public bool TryConnect()
    {
        try
        {
            _connection = _connectionFactory.CreateConnection();
            _connection.ConnectionShutdown += (sender, e) =>
            {
                if (_disposed) return;
                _logger.LogWarning("RabbitMQ connection shutdown. Trying to reconnect...");
                TryConnect();
            };
            _logger.LogInformation("RabbitMQ connection established");
            return true;
        }
        catch (SocketException ex)
        {
            _logger.LogError(ex, "RabbitMQ connection failed");
            return false;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _connection?.Dispose();
    }
}