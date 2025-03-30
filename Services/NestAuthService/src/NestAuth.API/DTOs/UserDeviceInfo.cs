namespace NestAuth.API.DTOs;

public class UserDeviceInfo
{
    public string IpAddress { get; set; }
    public string Browser { get; set; }
    public string DeviceType { get; set; }
    public string OperatingSystem { get; set; }
    public string DeviceName { get; set; }

    public UserDeviceInfo()
    {
        IpAddress = string.Empty;
        Browser = string.Empty;
        DeviceType = string.Empty;
        OperatingSystem = string.Empty;
        DeviceName = string.Empty;
    }
}