namespace NestAuth.API.DTOs.AuthDtos;

public class RefreshTokenRequest
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }

    public RefreshTokenRequest()
    {
        UserId = string.Empty;
        RefreshToken = string.Empty;
    }
}