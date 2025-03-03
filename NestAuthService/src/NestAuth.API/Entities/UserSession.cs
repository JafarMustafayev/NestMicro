namespace NestAuth.API.Entities;

public class UserSession : BaseEntityID
{
    public string UserId { get; set; }
    public string DeviceInfo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string CreatedByIp { get; set; }
    public string? RevokedByIp { get; set; }

    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
    public virtual AppUser User { get; set; }

    public UserSession()
    {
        UserId = string.Empty;
        DeviceInfo = string.Empty;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
        CreatedByIp = string.Empty;
        RevokedByIp = string.Empty;
        User = null!;
    }
}