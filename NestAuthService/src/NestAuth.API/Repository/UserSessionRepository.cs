namespace NestAuth.API.Repository;

public class UserSessionRepository : Repository<UserSession>, IUserSessionRepository
{
    public UserSessionRepository(AppDbContext context) : base(context)
    {
    }
}