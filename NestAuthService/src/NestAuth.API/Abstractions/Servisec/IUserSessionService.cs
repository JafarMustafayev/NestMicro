namespace NestAuth.API.Abstractions.Servisec;

public interface IUserSessionService
{
    Task<IEnumerable<UserSession>> GetUserSessionsAsync(string userId);

    Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId);

    Task<UserSession> CreateUserSessionAsync(UserSession userSession);

    Task<UserSession> RevokeUserSessionAsync(string sessionId, string revokedByIp);

    Task<UserSession> EndUserSessionAsync(string sessionId);

    Task<IEnumerable<UserSession>> GetUserSessionsByDeviceAsync(string userId, string deviceId);

    Task<IEnumerable<UserSession>> GetActiveUserSessionsAsync();
}