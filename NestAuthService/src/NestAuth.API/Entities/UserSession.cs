namespace NestAuth.API.Entities;

public class UserSession : BaseEntityID
{
    public string UserId { get; set; }
    public string DeviceId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string CreatedByIp { get; set; }
    public string RevokedByIp { get; set; }

    public virtual AppUser User { get; set; }
}