namespace NestAuth.API.Abstractions;

public interface ITokenHandler
{
    public Task<JwtTokenResponse> GenerateAccessTokenAsync(AppUser user);

    public string GenerateRefreshToken();
}