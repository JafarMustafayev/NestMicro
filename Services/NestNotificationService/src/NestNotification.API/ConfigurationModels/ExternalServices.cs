namespace NestNotification.API.ConfigurationModels;

public class ExternalServices
{
    public ApiGatewayConfig ApiGateway { get; set; } = new();
    public string ServerIp { get; set; } = string.Empty;
}

public class ApiGatewayConfig
{
    public ApiGatewayEnvironment Development { get; set; } = new();
    public ApiGatewayEnvironment Production { get; set; } = new();
}

public class ApiGatewayEnvironment
{
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
    public RetryPolicy RetryPolicy { get; set; } = new();
}

public class RetryPolicy
{
    public int MaxRetryAttempts { get; set; } = 3;
    public string DelayBetweenRetries { get; set; } = "00:00:02";
}