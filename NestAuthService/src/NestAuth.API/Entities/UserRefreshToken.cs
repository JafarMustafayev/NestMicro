namespace NestAuth.API.Entities;

public class UserRefreshToken : BaseEntityID
{
    public string UserId { get; set; }

    public string SessionId { get; set; }

    public string Token { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime Expires { get; set; }

    public bool IsUsed { get; set; } = false;

    public bool IsRevoked { get; set; } = false;

    public string CreatedByIp { get; set; }

    public string RevokedByIp { get; set; }
    public string ReplacedByTokenId { get; set; }

    public virtual AppUser User { get; set; }

    public virtual UserSession Session { get; set; }

    public virtual UserRefreshToken ReplacedByToken { get; set; }

    public UserRefreshToken()
    {
        UserId = string.Empty;
        SessionId = string.Empty;
        CreatedAt = DateTime.UtcNow;
        Token = string.Empty;
        Expires = DateTime.UtcNow;
        IsUsed = false;
        IsRevoked = false;
        ReplacedByTokenId = string.Empty;
        CreatedByIp = string.Empty;
        RevokedByIp = string.Empty;
    }
}