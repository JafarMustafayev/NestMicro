namespace NestStorage.API.Extensions;

public static class Configurations
{
    private static IConfiguration? _configuration;
    private static readonly object _lock = new();

    private static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                lock (_lock)
                {
                    if (_configuration == null)
                    {
                        _configuration = BuildConfiguration();
                    }
                }
            }

            return _configuration;
        }
    }

    private static IConfiguration BuildConfiguration()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var secretsJsonPath = GetSecretsPath();

        if (System.IO.File.Exists(secretsJsonPath))
        {
            configurationBuilder.AddJsonFile(secretsJsonPath, true, true);
        }

        configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
        configurationBuilder.AddJsonFile("appsettings.json", false, true);

        // Environment-specific appsettings
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        configurationBuilder.AddJsonFile($"appsettings.{environment}.json", true, true);

        return configurationBuilder.Build();
    }

    public static T GetConfiguration<T>() where T : class, new()
    {
        var sectionName = typeof(T).Name;
        var section = Configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");
        }

        var configObject = new T();
        section.Bind(configObject);

        return configObject;
    }

    public static T GetConfiguration<T>(string sectionName) where T : class, new()
    {
        var section = Configuration.GetSection(sectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{sectionName}' not found.");
        }

        var configObject = new T();
        section.Bind(configObject);

        return configObject;
    }

    // Connection string almaq üçün
    public static string GetConnectionString(string name = "DefaultConnection")
    {
        var connectionString = Configuration.GetConnectionString(name);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Connection string '{name}' not found.");
        }

        return connectionString;
    }

    private static string GetSecretsPath()
    {
        var userSecretsId = "ec19c56d-d737-482f-9baa-429e8a90170c";
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = Path.Combine(appDataPath, "Microsoft", "UserSecrets", userSecretsId, "secrets.json");
        return path;
    }

    public static bool IsProduction()
    {
        var aspNetCoreEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var serverIp = GetConfiguration<ExternalServices>().ServerIp;

        if (!string.IsNullOrEmpty(aspNetCoreEnv))
        {
            return aspNetCoreEnv.Equals("Production", StringComparison.OrdinalIgnoreCase) || InternetChecker.PingServer(serverIp);
        }

        return false;
    }
}