namespace NestAuth.API.Entities;

public class UserDevice : BaseEntityID
{
    public string UserId { get; set; }
    public string DeviceName { get; set; }
    public string DeviceType { get; set; }
    public string OperatingSystem { get; set; }
    public string UserAgent { get; set; }  // Brauzer və OS haqqında məlumat
    public string LastIpAddress { get; set; }
    public DateTime FirstLoginAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsBlocked { get; set; }

    public virtual AppUser User { get; set; }
    public virtual ICollection<UserSession> Sessions { get; set; } = new HashSet<UserSession>();

    public UserDevice()
    {
        UserId = string.Empty;
        DeviceName = string.Empty;
        DeviceType = string.Empty;
        OperatingSystem = string.Empty;
        UserAgent = string.Empty;
        LastIpAddress = string.Empty;
        FirstLoginAt = DateTime.UtcNow;
        LastLoginAt = DateTime.UtcNow;
        IsBlocked = false;
    }
}