namespace NestNotification.API.Services;

public class MailService : IMailService
{
    private readonly IEmailLogRepository _emailLogRepository;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailQueueRepository _failedEmailRepository;

    public MailService(
        IEmailLogRepository emailLogRepository,
        IEmailTemplateRepository emailTemplateRepository,
        IEmailQueueRepository failedEmailRepository)
    {
        _emailLogRepository = emailLogRepository;
        _emailTemplateRepository = emailTemplateRepository;
        _failedEmailRepository = failedEmailRepository;
    }

    public async Task<string> EmailSender(MailRequest mailRequest)
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

    public Task SendPriorityEmailAsync(SendEmailDto emailDto, EmailPriority priority = EmailPriority.High)
    {
        throw new NotImplementedException();
    }

    public Task SendPriorityTemplatedEmailAsync(SendTemplatedEmailDto emailDto, EmailPriority priority = EmailPriority.High)
    {
        throw new NotImplementedException();
    }

    public Task ScheduleEmailAsync(SendEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Normal)
    {
        throw new NotImplementedException();
    }

    public Task ScheduleTemplatedEmailAsync(SendTemplatedEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Normal)
    {
        throw new NotImplementedException();
    }

    public Task ScheduleBulkEmailAsync(SendBulkEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Low)
    {
        throw new NotImplementedException();
    }

    public Task ScheduleBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Low)
    {
        throw new NotImplementedException();
    }

    public Task ProcessEmailQueueAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<EmailQueueSummaryDto> GetEmailQueueSummaryAsync()
    {
        throw new NotImplementedException();
    }

    public Task<EmailTemplate> GetEmailTemplateByNameAsync(string templateName)
    {
        throw new NotImplementedException();
    }

    public Task RetryAllFailedEmailsAsync(int maxRetries = 3)
    {
        throw new NotImplementedException();
    }

    public Task CancelPendingEmailAsync(string emailId)
    {
        throw new NotImplementedException();
    }

    public Task CancelAllPendingEmailsAsync(string recipientEmail)
    {
        throw new NotImplementedException();
    }
}