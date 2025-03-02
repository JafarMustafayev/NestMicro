namespace NestNotification.API.Services;

public class MailService : IMailService
{
    public Task<string> EmailSender(MailRequest request)
    {
        throw new NotImplementedException();
    }

    public Task SendEmailAsync(SendEmailDto emailDto)
    {
        throw new NotImplementedException();
    }

    public Task SendTemplatedEmailAsync(SendTemplatedEmailDto emailDto)
    {
        throw new NotImplementedException();
    }

    public Task SendBulkEmailAsync(SendBulkEmailDto emailDto)
    {
        throw new NotImplementedException();
    }

    public Task SendBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto)
    {
        throw new NotImplementedException();
    }

    public Task<List<EmailLog>> GetEmailLogsAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<EmailStatus> GetEmailStatusAsync(string emailId)
    {
        throw new NotImplementedException();
    }

    public Task ResendFailedEmailAsync(string emailId)
    {
        throw new NotImplementedException();
    }

    public Task<List<EmailTemplate>> GetEmailTemplatesAsync()
    {
        throw new NotImplementedException();
    }

    public Task AddEmailTemplateAsync(CreateEmailTemplateDto templateDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateEmailTemplateAsync(UpdateEmailTemplateDto templateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmailTemplateAsync(string templateId)
    {
        throw new NotImplementedException();
    }
}