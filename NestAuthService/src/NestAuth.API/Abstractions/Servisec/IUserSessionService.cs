namespace NestAuth.API.Abstractions.Servisec;

public interface IUserSessionService
{
    IQueryable<UserSession> GetActiveSessionsByUser(string userId);

    Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId);

    IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId);

    Task<string> CreateSessionAsync(string userId, string deviceId, string createdByIp);

    Task<bool> RevokeSessionAsync(string sessionId, string revokedByIp);

    Task<bool> RevokeAllSessionsAsync(string userId, string revokedByIp);
}