namespace NestAuth.API.Services;

public class UserSessionService : IUserSessionService
{
    public Task<IEnumerable<UserSession>> GetUserSessionsAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<UserSession> CreateUserSessionAsync(UserSession userSession)
    {
        throw new NotImplementedException();
    }

    public Task<UserSession> RevokeUserSessionAsync(string sessionId, string revokedByIp)
    {
        throw new NotImplementedException();
    }

    public Task<UserSession> EndUserSessionAsync(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserSession>> GetUserSessionsByDeviceAsync(string userId, string deviceId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserSession>> GetActiveUserSessionsAsync()
    {
        throw new NotImplementedException();
    }
}