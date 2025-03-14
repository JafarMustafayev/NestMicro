namespace NestNotification.API.Repositories;

public class EmailTemplateAttributeRepository : Repository<EmailTemplateAttribute>, IEmailTemplateAttributeRepository
{
    public EmailTemplateAttributeRepository(AppDbContext context) : base(context)
    {
    }
}