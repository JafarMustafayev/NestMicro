namespace NestAuth.API.Context;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
}