namespace NestNotification.API.Repositories;

public class EmailQueueRepository : Repository<EmailQueue>, IEmailQueueRepository
{
    public EmailQueueRepository(AppDbContext context) : base(context)
    {
    }
}