namespace NestAuth.API.Repositories;

public class TokenRepository : Repository<UserRefreshToken>, ITokenRepository
{
    public TokenRepository(AppDbContext context) : base(context)
    {
    }
}