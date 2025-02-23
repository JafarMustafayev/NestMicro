namespace NestAuth.API.Entities;

public class UserDevice : BaseEntityID
{
    public string UserId { get; set; }  // İstifadəçi ID (IdentityUser ilə əlaqəli)
    public string DeviceName { get; set; }  // Cihaz adı (Məs: "iPhone 14", "Samsung Galaxy S23")
    public string DeviceType { get; set; }  // Cihaz növü ("Mobile", "Desktop", "Tablet")
    public string OperatingSystem { get; set; }  // OS adı (Windows, macOS, Android, iOS)
    public string UserAgent { get; set; }  // Brauzer və OS haqqında məlumat
    public string LastIpAddress { get; set; }  // Son IP ünvanı
    public DateTime FirstLoginAt { get; set; }  // İlk dəfə nə vaxt daxil olub?
    public DateTime LastLoginAt { get; set; }  // Sonuncu dəfə nə vaxt daxil olub?
    public bool IsBlocked { get; set; }  // Admin tərəfindən blok edilibmi?

    public virtual AppUser User { get; set; }  // IdentityUser ilə əlaqə
    public virtual ICollection<UserSession> Sessions { get; set; }  // İstifadəçinin sessiyaları

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