﻿namespace NestAuth.API;

public static class Configuration
{
    internal static string GetConfiguratinValue(string section)
    {
        ConfigurationManager configurationManager = new();

        configurationManager.SetBasePath(GetSecretsPath());
        configurationManager.AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

        var value = configurationManager.GetSection(section).Value;

        if (value != null)
        { return value; }
        else
        {
            configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            value = configurationManager.GetSection(section).Value;
            ;

            if (value != null)
            { return value; }
            else
            { throw new Exception($"'{section}' is not found "); }
        }
    }

    internal static string GetConfiguratinValue(string section, string subSection)
    {
        ConfigurationManager configurationManager = new();

        configurationManager.SetBasePath(GetSecretsPath());
        configurationManager.AddJsonFile("Secrets.json", optional: true, reloadOnChange: true);

        var str = configurationManager.GetSection(section);

        var value = str.GetValue<string>(subSection);

        if (value is not null)
        {
            return value;
        }
        else
        {
            configurationManager.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            configurationManager.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            str = configurationManager.GetSection(section);
            value = str.GetValue<string>(subSection);

            if (str != null || value != null)
            { return value; }
            else
            { throw new Exception($"'{section}' or '{subSection}' is not found "); }
        }
    }

    private static string GetSecretsPath()
    {
        string userSecretsId = "97dedcf8-f624-4053-984e-960fd0233215"; // Layihə üçün UserSecrets ID-si
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var path = Path.Combine(appDataPath, "Microsoft", "UserSecrets", userSecretsId);
        return path;
    }
}