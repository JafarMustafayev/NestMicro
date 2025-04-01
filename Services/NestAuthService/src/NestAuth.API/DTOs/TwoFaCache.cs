namespace NestAuth.API.DTOs;

public class TwoFaCache
{
    public string Otp { get; set; }
    public string TemporaryToken { get; set; }
    public string UserId { get; set; }
    public TimeSpan Expiration { get; set; }

    public TwoFaCache()
    {
        Otp = string.Empty;
        TemporaryToken = string.Empty;
        UserId = string.Empty;
    }
}