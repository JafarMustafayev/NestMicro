namespace NestAuth.API.Entities;

public class UserRefreshToken : BaseEntityId
{
    public string UserId { get; set; }

    public string SessionId { get; set; }

    public string Token { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime Expires { get; set; }

    public bool IsUsed { get; set; }

    public bool IsRevoked { get; set; }

    public string CreatedByIp { get; set; }

    public string? RevokedByIp { get; set; }

    public virtual AppUser User { get; set; }

    public virtual UserSession Session { get; set; }

    public UserRefreshToken()
    {
        UserId = string.Empty;
        SessionId = string.Empty;
        Token = $"{Guid.NewGuid()}-{Guid.NewGuid()}";
        CreatedAt = DateTime.UtcNow;
        Expires = DateTime.UtcNow;
        IsUsed = false;
        IsRevoked = false;
        CreatedByIp = string.Empty;
        RevokedByIp = string.Empty;
        User = null!;
        Session = null!;
    }
}