namespace NestAuth.API.ConfigurationModels;

public class MessageBroker
{
    public RabbitMQConfig RabbitMQ { get; set; } = new();
}

public class RabbitMQConfig
{
    public RabbitMQEnvironment Development { get; set; } = new();
    public RabbitMQEnvironment Production { get; set; } = new();
}

public class RabbitMQEnvironment
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = "/";
    public int ConnectionTimeout { get; set; } = 30;
    public int RequestedHeartbeat { get; set; } = 60;
    public bool DispatchConsumersAsync { get; set; } = true;
    public bool AutomaticRecoveryEnabled { get; set; } = true;
}