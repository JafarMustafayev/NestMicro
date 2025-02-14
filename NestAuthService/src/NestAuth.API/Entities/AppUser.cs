namespace NestAuth.API.Entities;

public class AppUser : IdentityUser<string>
{
    public DateTime CreatedAt { get; set; }

    public UserStatus UserStatus { get; set; }

    public AppUser()
    {
        Id = Guid.NewGuid().ToString();
        UserStatus = UserStatus.PendingVerification;
    }
}