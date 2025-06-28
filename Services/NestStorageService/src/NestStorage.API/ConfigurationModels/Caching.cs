namespace NestStorage.API.ConfigurationModels;

public class Caching
{
    public RedisConfig Redis { get; set; } = new();
    public InMemoryConfig InMemory { get; set; } = new();
}

public class RedisConfig
{
    public RedisEnvironment Development { get; set; } = new();
    public RedisEnvironment Production { get; set; } = new();
}

public class RedisEnvironment
{
    public string ConnectionString { get; set; } = string.Empty;
    public int DefaultExpirationMinutes { get; set; } = 30;
    public string KeyPrefix { get; set; } = string.Empty;
}

public class InMemoryConfig
{
    public int SizeLimit { get; set; } = 100;
    public double CompactionPercentage { get; set; } = 0.25;
}