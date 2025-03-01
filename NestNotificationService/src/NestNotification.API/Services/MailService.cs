namespace NestNotification.API.Services;

public class MailService : IMailService
{
    public Task SendEmailAsync(string to, string subject, string body, bool isHtml = true, byte[] fileData = null, string fileName = null, string fileMimeType = null)
    {
        throw new NotImplementedException();
    }

    public Task SendTemplatedEmailAsync(string to, string templateId, Dictionary<string, string> placeholders, byte[] fileData = null, string fileName = null, string fileMimeType = null)
    {
        throw new NotImplementedException();
    }

    public Task SendBulkEmailAsync(List<string> recipients, string subject, string body, bool isHtml = true, byte[] fileData = null, string fileName = null, string fileMimeType = null)
    {
        throw new NotImplementedException();
    }

    public Task SendBulkTemplatedEmailAsync(List<string> recipients, string templateId, Dictionary<string, string> placeholders, byte[] fileData = null, string fileName = null, string fileMimeType = null)
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

    public Task AddEmailTemplateAsync(EmailTemplate template)
    {
        throw new NotImplementedException();
    }

    public Task UpdateEmailTemplateAsync(EmailTemplate template)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmailTemplateAsync(string templateId)
    {
        throw new NotImplementedException();
    }
}