namespace NestAuth.API.Abstractions.Servisec;

public interface IUserSessionService
{
    public IQueryable<UserSession> GetActiveSessionsByUser(string userId);

    public Task<UserSession> GetUserSessionByIdAsync(string userId, string sessionId);

    //public IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId);

    public Task<string> CreateSessionAsync(string userId);

    public Task<bool> RevokeSessionAsync(string sessionId);

    public Task<bool> RevokeAllSessionsAsync(string userId);
}