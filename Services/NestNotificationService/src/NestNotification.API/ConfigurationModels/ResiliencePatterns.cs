namespace NestNotification.API.ConfigurationModels;

public class ResiliencePatterns
{
    public RetryPolicyConfig RetryPolicy { get; set; } = new();
    public CircuitBreakerConfig CircuitBreaker { get; set; } = new();
    public TimeoutConfig Timeout { get; set; } = new();
}

public class RetryPolicyConfig
{
    public int MaxRetryAttempts { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 20000;
    public double BackoffMultiplier { get; set; } = 2.0;
    public int MaxDelayMilliseconds { get; set; } = 60000;
}

public class CircuitBreakerConfig
{
    public int HandledEventsAllowedBeforeBreaking { get; set; } = 5;
    public int DurationOfBreakMilliseconds { get; set; } = 30000;
    public int SamplingDurationMilliseconds { get; set; } = 60000;
    public int MinimumThroughput { get; set; } = 10;
}

public class TimeoutConfig
{
    public int DefaultTimeoutMilliseconds { get; set; } = 30000;
}