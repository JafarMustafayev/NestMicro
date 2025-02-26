namespace NestAuth.API.Services;

public class UserSessionService : IUserSessionService
{
    private readonly IUserSessionRepository _userSessionRepository;

    public UserSessionService(
        IUserSessionRepository userSessionRepository)
    {
        _userSessionRepository = userSessionRepository;
    }

    public IQueryable<UserSession> GetActiveSessionsByUser(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId)
    {
        throw new NotImplementedException();
    }

    public IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<string> CreateSessionAsync(string userId, string deviceId, string createdByIp)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RevokeSessionAsync(string sessionId, string revokedByIp)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RevokeAllSessionsAsync(string userId, string revokedByIp)
    {
        throw new NotImplementedException();
    }
}