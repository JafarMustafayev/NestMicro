namespace NestNotification.API.Abstractions.Servisec;

public interface IMailService
{
    Task<string> EmailSender(MailRequest request);

    Task SendEmailAsync(SendEmailDto emailDto);

    Task SendTemplatedEmailAsync(SendTemplatedEmailDto emailDto);

    Task SendBulkEmailAsync(SendBulkEmailDto emailDto);

    Task SendBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto);

    Task<List<EmailLog>> GetEmailLogsAsync(int page, int pageSize);

    Task<EmailStatus> GetEmailStatusAsync(string emailId);

    Task ResendFailedEmailAsync(string emailId);

    Task<List<EmailTemplate>> GetEmailTemplatesAsync();

    Task AddEmailTemplateAsync(CreateEmailTemplateDto templateDto);

    Task UpdateEmailTemplateAsync(UpdateEmailTemplateDto templateDto);

    Task DeleteEmailTemplateAsync(string templateId);
}