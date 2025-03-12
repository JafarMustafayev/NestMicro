namespace NestNotification.API.Extensions;

public static class Configurations
{
    internal static T GetConfiguratinValue<T>(string section)
    {
        ConfigurationManager configurationManager = new();

        configurationManager.SetBasePath(GetSecretsPath());
        configurationManager.AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

        var value = configurationManager.GetValue<T>(section);

        if (value != null)
        { return value; }
        else
        {
            configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            value = configurationManager.GetValue<T>(section);
            ;

            if (value != null)
            { return value; }
            else
            { throw new Exception($"'{section}' is not found "); }
        }
    }

    internal static T GetConfiguratinValue<T>(string section, string subSection)
    {
        ConfigurationManager configurationManager = new();

        configurationManager.SetBasePath(GetSecretsPath());
        configurationManager.AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

        var str = configurationManager.GetSection(section);

        var value = str.GetValue<T>(subSection);

        if (value is not null)
        {
            return value;
        }
        else
        {
            configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            str = configurationManager.GetSection(section);
            value = str.GetValue<T>(subSection);

            if (str != null && value != null)
            { return value; }
            else
            { throw new Exception($"'{section}' or '{subSection}' is not found "); }
        }
    }

    internal static T GetConfiguratinValue<T>(string section, string secondSection, string subSection)
    {
        ConfigurationManager configurationManager = new();

        configurationManager.SetBasePath(GetSecretsPath());
        configurationManager.AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

        var sct = configurationManager.GetSection(section);

        var subsct = sct.GetSection(secondSection);

        var value = subsct.GetValue<T>(subSection);

        if (value is not null)
        {
            return value;
        }
        else
        {
            configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            sct = configurationManager.GetSection(section);

            subsct = sct.GetSection(secondSection);

            value = subsct.GetValue<T>(subSection);

            if (value is not null)
            {
                return value;
            }
            else
            { throw new Exception($"'{section}' or '{subSection}' is not found "); }
        }
    }

    private static string GetSecretsPath()
    {
        string userSecretsId = "174d85dd-3f9b-4807-9491-679f0890e023"; // Layihə üçün UserSecrets ID-si
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = Path.Combine(appDataPath, "Microsoft", "UserSecrets", userSecretsId);
        return path;
    }
}