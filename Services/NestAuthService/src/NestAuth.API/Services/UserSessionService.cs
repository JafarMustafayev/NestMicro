namespace NestAuth.API.Services;

public class UserSessionService : IUserSessionService
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IUserDeviceInfoService _userDeviceInfoService;
    private readonly ITokenService _tokenService;

    public UserSessionService(
        IUserSessionRepository userSessionRepository,
        IUserDeviceInfoService userDeviceInfoService,
        ITokenService tokenService)
    {
        _userSessionRepository = userSessionRepository;
        _userDeviceInfoService = userDeviceInfoService;
        _tokenService = tokenService;
    }

    public ResponseDto GetActiveSessionsByUser(string userId)
    {
        Expression<Func<UserSession, bool>> expression = x => x.UserId == userId && !x.IsRevoked;

        var res = _userSessionRepository.GetAllByExpression(expression);

        if (res.Count == 0)
        {
            throw new EntityNotFoundException();
        }

        var dtos = new List<UserSessionDto>();

        foreach (var d in res.Items.Where(x => x != null).Cast<UserSession>())
        {
            dtos.Add(new UserSessionDto
            {
                Id = d.Id,
                UserId = d.UserId,
                DeviceInfo = d.DeviceInfo,
                LocationInfo = d.LocationInfo,
                CreatedAt = d.CreatedAt,
                IsRevoked = d.IsRevoked,
                ExpiresAt = d.ExpiresAt
            });

            //todo: add auto mapping 
        }

        return new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = dtos,
            Message = "Active sessions retrieved successfully"
        };
    }

    public async Task<ResponseDto> GetUserSessionByIdAsync(string userId, string sessionId)
    {
        Expression<Func<UserSession, bool>> expression = x => x.UserId == userId && !x.IsRevoked && x.Id == sessionId;

        var res = await _userSessionRepository.GetByExpressionAsync(expression);

        if (res == null)
        {
            throw new EntityNotFoundException();
        }

        return new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = new UserSessionDto()
            {
                Id = res.Id,
                UserId = res.UserId,
                DeviceInfo = res.DeviceInfo,
                CreatedAt = res.CreatedAt,
                IsRevoked = res.IsRevoked,
                ExpiresAt = res.ExpiresAt
            },
            Message = "Session retrieved successfully"
        };
    }

    //public IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId)
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<(string sessionId, NewUserLoginDetectedIntegrationEvent @event)> CreateSessionAsync(AppUser user)
    {
        var deviceInfo = _userDeviceInfoService.GetUserDeviceInfo();
        var locationInfo = await _userDeviceInfoService.GetUserLocationInfo();

        UserSession session = new()
        {
            UserId = user.Id,
            CreatedByIp = _userDeviceInfoService.GetClientIp(),
            DeviceInfo = new
            {
                DeviceType = deviceInfo.DeviceType,
                Browser = deviceInfo.Browser,
                Os = deviceInfo.OperatingSystem,
                DeviceName = deviceInfo.DeviceName,
            }.ToString(),
            LocationInfo = new
            {
                CountryName = locationInfo.CountryName,
                RegionName = locationInfo.RegionName,
                CityName = locationInfo.CityName,
                Latitude = locationInfo.Latitude,
                Longitude = locationInfo.Longitude,
                TimeZone = locationInfo.TimeZone,
            }.ToString(),
            //todo: add auto mapping 
        };

        var timeZoneOffset = locationInfo.TimeZone != null ? locationInfo.TimeZone : "+ 00:00";
        string parsableOffset = timeZoneOffset.StartsWith("+") ? timeZoneOffset.Substring(1) : timeZoneOffset;
        var offset = TimeSpan.Parse(parsableOffset);

        var localTime = DateTime.UtcNow + offset;

        NewUserLoginDetectedIntegrationEvent @event = new()
        {
            Email = user?.Email ?? string.Empty,
            UserName = user?.UserName ?? string.Empty,
            IpAddress = _userDeviceInfoService.GetClientIp() ?? "-",
            Country = locationInfo.CountryName ?? "-",
            City = locationInfo.CityName ?? "-",
            LocalTime = localTime.ToString("dd/MM/yyyy HH:mm") ?? "-",
            UtcTime = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
            TimeZone = locationInfo.TimeZone ?? "-",
            DeviceName = deviceInfo.DeviceName ?? "-",
            DeviceType = deviceInfo.DeviceType ?? "-",
            OperatingSystem = deviceInfo.OperatingSystem ?? "-",
            ManageAccountUrl = "/account/manage"
        };

        await _userSessionRepository.AddAsync(session);
        await _userSessionRepository.SaveChangesAsync();
        return (session.Id, @event);
    }

    public async Task<ResponseDto> RevokeSessionAsync(string sessionId)
    {
        Expression<Func<UserSession, bool>> expression = x => !x.IsRevoked && x.Id == sessionId;

        var res = await _userSessionRepository.GetByExpressionAsync(expression);

        if (res == null)
        {
            throw new EntityNotFoundException();
        }

        res.IsRevoked = true;
        res.RevokedByIp = _userDeviceInfoService.GetClientIp();
        res.ExpiresAt = DateTime.UtcNow;

        _userSessionRepository.Update(res);

        await _userSessionRepository.SaveChangesAsync();

        return new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "Session revoked successfully"
        };
    }

    public async Task<ResponseDto> RevokeAllSessionsAsync(string userId)
    {
        var res = _userSessionRepository.GetAllByExpression(x => !x.IsRevoked);

        if (res.Count == 0)
        {
            throw new EntityNotFoundException();
        }

        foreach (var session in res.Items.Where(x => x != null))
        {
            session.IsRevoked = true;
            session.RevokedByIp = _userDeviceInfoService.GetClientIp();
            session.ExpiresAt = DateTime.UtcNow;

            _userSessionRepository.Update(session);
        }

        await _userSessionRepository.SaveChangesAsync();

        await _tokenService.RevokeUserRefreshAllTokens(userId);

        return new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Message = "All sessions revoked successfully"
        };
    }
}