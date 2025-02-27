namespace NestAuth.API.Abstractions.Servisec;

public interface ITokenService
{
    public Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user, string sessionId);

    public Task<UserRefreshToken> GenerateRefreshTokenAsync(string userId, string sessionId);

    public Task<string> GetSessionId(string userId, string refreshToken);

    public Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);

    public Task MarkRefreshTokenAsUsedAsync(string userId, string refreshToken);

    public Task RevokeRefreshTokenAsync(string userId, string refreshToken);
}