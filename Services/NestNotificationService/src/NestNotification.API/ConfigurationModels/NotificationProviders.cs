namespace NestNotification.API.ConfigurationModels;

public class NotificationProviders
{
    public EmailProviders Email { get; set; } = new();
    public SmsProvider Sms { get; set; } = new();
}

public class EmailProviders
{
    public EmailProvider Primary { get; set; } = new();
    public EmailProvider Secondary { get; set; } = new();
}

public class EmailProvider
{
    public string Provider { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string FromEmail { get; set; } = string.Empty;
    public SmtpSettings SmtpSettings { get; set; } = new();
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    public bool UseDefaultCredentials { get; set; } = false;
    public int TimeoutInSeconds { get; set; }
}

public class SmsProvider
{
    public string Provider { get; set; } = string.Empty;
    public SmsSettings Settings { get; set; } = new();
}

public class SmsSettings
{
    public string AccountSid { get; set; } = string.Empty;
    public string AuthToken { get; set; } = string.Empty;
    public string FromNumber { get; set; } = string.Empty;
}