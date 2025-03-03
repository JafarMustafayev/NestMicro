namespace NestAuth.API.Services;

public class UserDeviceInfoService : IUserDeviceInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserDeviceInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetClientIp()
    {
        var headers = _httpContextAccessor?.HttpContext?.Request.Headers;

        if (headers == null)
        {
            return string.Empty;
        }

        var ip = headers["X-Forwarded-For"].FirstOrDefault()
                 ?? _httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();

        if (ip != null && ip.Contains("::ffff:"))
        {
            ip = ip.Replace("::ffff:", "");  // IPv6 mapped IPv4 formatını təmizlə
            return ip == "::1" ? "127.0.0.1" : ip;
        }

        return string.Empty;
    }

    public string GetUserAgent()
    {
        return _httpContextAccessor?.HttpContext?.Request?.Headers.UserAgent.ToString() ?? string.Empty;
    }

    public string GetDeviceType()
    {
        var agent = GetUserAgent();

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(agent);

        return clientInfo.Device.Family.ToString();
    }

    public string GetBrowser()
    {
        var agent = GetUserAgent();

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(agent);

        return $"{clientInfo.UA.Family} {clientInfo.UA.Major}";
    }

    public string GetOs()
    {
        var agent = GetUserAgent();

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(agent);

        return $"{clientInfo.OS.Family} {clientInfo.OS.Major}";
    }

    public string GetDeviceName()
    {
        var agent = GetUserAgent();

        var uaParser = Parser.GetDefault();
        var clientInfo = uaParser.Parse(agent);

        return clientInfo.Device.ToString();
    }
}