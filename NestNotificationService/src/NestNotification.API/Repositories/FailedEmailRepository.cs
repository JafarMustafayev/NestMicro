namespace NestNotification.API.Repositories;

public class FailedEmailRepository : Repository<FailedEmail>, IFailedEmailRepository
{
    public FailedEmailRepository(AppDbContext context) : base(context)
    {
    }
}