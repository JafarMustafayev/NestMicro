namespace EventBus.Abstractions.Events;

public class NewUserLoginDetectedIntegrationEvent : IntegrationEvent
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string IpAddress { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string LocalTime { get; set; }
    public string UtcTime { get; set; }
    public string TimeZone { get; set; }
    public string DeviceType { get; set; }
    public string DeviceName { get; set; }
    public string OperatingSystem { get; set; }
    public string ManageAccountUrl { get; set; }

    public NewUserLoginDetectedIntegrationEvent()
    {
        UserName = string.Empty;
        Email = string.Empty;
        IpAddress = string.Empty;
        Country = string.Empty;
        City = string.Empty;
        LocalTime = string.Empty;
        UtcTime = string.Empty;
        TimeZone = string.Empty;
        DeviceType = string.Empty;
        DeviceName = string.Empty;
        OperatingSystem = string.Empty;
        ManageAccountUrl = string.Empty;
    }
}