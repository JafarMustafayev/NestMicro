namespace NestAuth.API.DTOs;

public class Verify2FACodeRequest
{
    public string UserId { get; set; }
    public string Code { get; set; }
    public string TemporaryToken { get; set; }

    public Verify2FACodeRequest()
    {
        Code = string.Empty;
        TemporaryToken = string.Empty;
        UserId = string.Empty;
    }
}