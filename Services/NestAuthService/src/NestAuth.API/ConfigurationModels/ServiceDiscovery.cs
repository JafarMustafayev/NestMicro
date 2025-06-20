namespace NestAuth.API.ConfigurationModels;

public class ServiceDiscovery
{
    public ConsulConfig Consul { get; set; } = new();
}

public class ConsulConfig
{
    public ConsulEndpoints Endpoints { get; set; } = new();
    public ServiceRegistration ServiceRegistration { get; set; } = new();
    public HealthCheck HealthCheck { get; set; } = new();
}

public class ConsulEndpoints
{
    public string Development { get; set; } = string.Empty;
    public string Production { get; set; } = string.Empty;
}

public class ServiceRegistration
{
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Port { get; set; }
    public string[] Tags { get; set; } = Array.Empty<string>();
    public Dictionary<string, string> Meta { get; set; } = new();
    public bool EnableTagOverride { get; set; }
}

public class HealthCheck
{
    public HealthCheckEndpoints Endpoints { get; set; } = new();
    public string HealthCheckPath { get; set; } = "/health";
    public int Interval { get; set; } = 10;
    public int Timeout { get; set; } = 5;
    public int DeregisterCriticalServiceAfter { get; set; } = 30;
    public bool TLSSkipVerify { get; set; } = false;
}

public class HealthCheckEndpoints
{
    public string Development { get; set; } = string.Empty;
    public string Production { get; set; } = string.Empty;
}