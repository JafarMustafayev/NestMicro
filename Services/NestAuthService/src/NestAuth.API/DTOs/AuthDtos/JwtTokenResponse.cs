namespace NestAuth.API.DTOs.AuthDtos;

public class JwtTokenResponse
{
    public string AccessToken { get; set; }
    public DateTime ExpiresIn { get; set; }

    public DateTime IssuedAt { get; set; }
    public string RefreshToken { get; set; }

    public JwtTokenResponse()
    {
        AccessToken = string.Empty;
        ExpiresIn = DateTime.MinValue;
        IssuedAt = DateTime.MinValue;
        RefreshToken = string.Empty;
    }
}