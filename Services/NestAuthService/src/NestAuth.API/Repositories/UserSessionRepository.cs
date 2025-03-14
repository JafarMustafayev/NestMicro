namespace NestAuth.API.Repositories;

public class UserSessionRepository : Repository<UserSession>, IUserSessionRepository
{
    public UserSessionRepository(AppDbContext context) : base(context)
    {
    }
}