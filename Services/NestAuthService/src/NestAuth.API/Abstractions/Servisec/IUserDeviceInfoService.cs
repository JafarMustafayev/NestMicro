namespace NestAuth.API.Abstractions.Servisec;

public interface IUserDeviceInfoService
{
    string GetClientIp();
    UserDeviceInfo GetUserDeviceInfo();
    Task<UserLocationInfo> GetUserLocationInfo(string? ipAddress = null);
}