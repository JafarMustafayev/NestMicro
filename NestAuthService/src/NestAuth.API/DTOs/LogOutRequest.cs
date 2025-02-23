namespace NestAuth.API.DTOs;

public class LogOutRequest
{
    public string UserId { get; set; }
    public string RefreshToken { get; set; }
}