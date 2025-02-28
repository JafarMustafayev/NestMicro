namespace NestNotification.API.Repositories;

public class EmailLogRepository : Repository<EmailLog>, IEmailLogRepository
{
    public EmailLogRepository(AppDbContext context) : base(context)
    {
    }
}