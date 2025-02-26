namespace NestAuth.API.Entities;

public class AppUser : IdentityUser<string>
{
    public DateTime CreatedAt { get; set; }

    public UserStatus UserStatus { get; set; }

    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
    public virtual ICollection<UserSession> Sessions { get; set; } = new HashSet<UserSession>();
    public virtual ICollection<UserDevice> Devices { get; set; } = new HashSet<UserDevice>();

    public AppUser()
    {
        UserStatus = UserStatus.PendingVerification;
    }
}