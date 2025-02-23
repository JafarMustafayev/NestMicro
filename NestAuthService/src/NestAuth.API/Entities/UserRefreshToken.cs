namespace NestAuth.API.Entities;

public class UserRefreshToken : BaseEntityID
{
    public string UserId { get; set; }

    public string SessionId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }

    public bool IsUsed { get; set; } = false;  // IsUsed is used to check if the token is used

    public bool IsRevoked { get; set; } = false; // IsRevoked is used to check if the token is revoked

    public string ReplacedByTokenId { get; set; }  // ReplacedByTokenId is used to replace the token by token ID

    public string CreatedByIp { get; set; }  // CreatedByIp is used to create the token by IP address of the user

    public string RevokedByIp { get; set; }  // RevokedByIp is used to revoke the token by IP address of the user

    public virtual AppUser User { get; set; }

    public virtual UserSession Session { get; set; }

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