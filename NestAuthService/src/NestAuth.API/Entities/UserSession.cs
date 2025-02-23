namespace NestAuth.API.Entities;

public class UserSession : BaseEntityID
{
    public string UserId { get; set; }  // İstifadəçi ID
    public string DeviceId { get; set; }  // UserDevice ID (Hansı cihazdan login olunub)
    public DateTime CreatedAt { get; set; }  // Sessiya yaradılma tarixi
    public DateTime ExpiresAt { get; set; }  // Sessiyanın bitmə tarixi
    public bool IsRevoked { get; set; } = false;  // Sessiya ləğv olunubmu?
    public string CreatedByIp { get; set; }  // IP ünvanı
    public string RevokedByIp { get; set; }  // Sessiya bağlananda hansı IP-dən edilib?

    public virtual AppUser User { get; set; }  // IdentityUser ilə əlaqə
    public virtual UserDevice Device { get; set; }  // Hansi cihazdan daxil olunub
}