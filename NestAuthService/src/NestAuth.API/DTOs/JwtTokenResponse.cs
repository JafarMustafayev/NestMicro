namespace NestAuth.API.DTOs;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public DateTime ExpiresIn { get; set; }

    public DateTime IssuedAt { get; set; }
    public string RefreshToken { get; set; }
}