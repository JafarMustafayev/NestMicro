namespace NestAuth.API.DTOs;

public class RegisterDeviceDto
{
    public string UserId { get; set; }

    public string DeviceName { get; set; }

    public string DeviceType { get; set; }

    public string OperatingSystem { get; set; }

    public string UserAgent { get; set; }

    public string IpAddress { get; set; }
}