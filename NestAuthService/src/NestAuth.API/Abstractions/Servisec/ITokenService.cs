namespace NestAuth.API.Abstractions.Servisec;

public interface ITokenService
{
    public Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user);

    public Task<UserRefreshToken> GenerateRefreshTokenAsync(string userId);

    public Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);

    public Task MarkRefreshTokenAsUsedAsync(string userId, string refreshToken);

    public Task RevokeRefreshTokenAsync(string userId, string refreshToken);
}