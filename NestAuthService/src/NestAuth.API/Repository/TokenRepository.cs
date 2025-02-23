namespace NestAuth.API.Repository;

public class TokenRepository : Repository<UserRefreshToken>, ITokenRepository
{
    public TokenRepository(AppDbContext context) : base(context)
    {
    }
}