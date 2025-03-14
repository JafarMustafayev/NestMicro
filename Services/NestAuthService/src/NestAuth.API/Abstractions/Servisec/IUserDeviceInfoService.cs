namespace NestAuth.API.Abstractions.Servisec;

public interface IUserDeviceInfoService
{
    string GetClientIp();

    string GetUserAgent();

    string GetDeviceType();

    string GetBrowser();

    string GetOs();

    string GetDeviceName();
}