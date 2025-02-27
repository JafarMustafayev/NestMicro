namespace NestAuth.API.Services;

public class UserSessionService : IUserSessionService
{
    private readonly IUserSessionRepository _userSessionRepository;
    private readonly IUserDeviceInfoService _userDeviceInfoService;

    public UserSessionService(
        IUserSessionRepository userSessionRepository,
        IUserDeviceInfoService userDeviceInfoService)
    {
        _userSessionRepository = userSessionRepository;
        _userDeviceInfoService = userDeviceInfoService;
    }

    public IQueryable<UserSession> GetActiveSessionsByUser(string userId)
    {
        Expression<Func<UserSession, bool>> expression = x => x.UserId == userId && x.ExpiresAt > DateTime.UtcNow && !x.IsRevoked;

        var res = _userSessionRepository.GetAllByExpression(expression);

        if (res.Count == 0)
        {
            throw new EntityNotFoundException();
        }

        return res.Items.Where(x => x != null).Cast<UserSession>();
    }

    public async Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId)
    {
        Expression<Func<UserSession, bool>> expression = x => x.UserId == userId && x.ExpiresAt > DateTime.UtcNow && !x.IsRevoked && x.Id == sessionId;

        var res = await _userSessionRepository.GetByExpressionAsync(expression);

        if (res == null)
        {
            throw new EntityNotFoundException();
        }

        return res;
    }

    //public IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId)
    //{
    //    throw new NotImplementedException();
    //}

    public async Task<string> CreateSessionAsync(string userId)
    {
        UserSession session = new()
        {
            UserId = userId,
            DeviceInfo = new
            {
                DeviceType = _userDeviceInfoService.GetDeviceType(),
                Browser = _userDeviceInfoService.GetBrowser(),
                Os = _userDeviceInfoService.GetOs(),
                DeviceName = _userDeviceInfoService.GetDeviceName(),
            }.ToString(),
            CreatedByIp = _userDeviceInfoService.GetClientIp()
        };

        await _userSessionRepository.AddAsync(session);
        await _userSessionRepository.SaveChangesAsync();
        return session.Id;
    }

    public async Task<bool> RevokeSessionAsync(string sessionId)
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
        return true;
    }

    public async Task<bool> RevokeAllSessionsAsync(string userId)
    {
        Expression<Func<UserSession, bool>> expression = x => x.ExpiresAt > DateTime.UtcNow && !x.IsRevoked;

        var res = _userSessionRepository.GetAllByExpression(expression);

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
        return true;
    }
}