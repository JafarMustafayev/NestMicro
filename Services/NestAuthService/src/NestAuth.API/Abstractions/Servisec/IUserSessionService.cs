namespace NestAuth.API.Abstractions.Servisec;

public interface IUserSessionService
{
    public ResponseDto GetActiveSessionsByUser(string userId);

    public Task<ResponseDto> GetUserSessionByIdAsync(string userId, string sessionId);

    //public IQueryable<UserSession> GetUserSessionsByDevice(string userId, string deviceId);

    public Task<string> CreateSessionAsync(string userId);

    public Task<ResponseDto> RevokeSessionAsync(string sessionId);

    public Task<ResponseDto> RevokeAllSessionsAsync(string userId);
}