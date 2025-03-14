namespace NestAuth.API.Entities;

public class AppRole : IdentityRole<string>
{
    public AppRole()
    {
        Id = Guid.NewGuid().ToString();
    }
}