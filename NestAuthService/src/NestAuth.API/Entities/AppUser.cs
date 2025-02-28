namespace NestAuth.API.Entities;

public class AppUser : IdentityUser<string>
{
    public DateTime CreatedAt { get; set; }

    public UserStatus UserStatus { get; set; }

    public virtual ICollection<UserRefreshToken> RefreshTokens { get; set; } = new HashSet<UserRefreshToken>();
    public virtual ICollection<UserSession> Sessions { get; set; } = new HashSet<UserSession>();

    public AppUser()
    {
        Id = Guid.NewGuid().ToString();
        UserStatus = UserStatus.PendingVerification;
    }
}