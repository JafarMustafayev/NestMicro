namespace NestNotification.API.Abstractions.Services;

public interface IMailService
{
    // Mövcud metodlar
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

    // Prioritet və planlaşdırılmış email göndərmə metodları
    Task SendPriorityEmailAsync(SendEmailDto emailDto, EmailPriority priority = EmailPriority.High);

    Task SendPriorityTemplatedEmailAsync(SendTemplatedEmailDto emailDto, EmailPriority priority = EmailPriority.High);

    // Planlaşdırılmış email göndərmə metodları
    Task ScheduleEmailAsync(SendEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Normal);

    Task ScheduleTemplatedEmailAsync(SendTemplatedEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Normal);

    Task ScheduleBulkEmailAsync(SendBulkEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Low);

    Task ScheduleBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto, DateTime scheduledAt, EmailPriority priority = EmailPriority.Low);

    // Queue emalı metodu (əgər ayrıca consumer servisi olmasa)
    Task ProcessEmailQueueAsync(CancellationToken cancellationToken);

    // Email statuslarının yığcam analizi
    Task<EmailQueueSummaryDto> GetEmailQueueSummaryAsync();

    // Templatelərə aid əlavə funksiyalar
    Task<EmailTemplate> GetEmailTemplateByNameAsync(string templateName);

    // Fasiləyə uğramış emaillərin idarə edilməsi
    Task RetryAllFailedEmailsAsync(int maxRetries = 3);

    Task CancelPendingEmailAsync(string emailId);

    Task CancelAllPendingEmailsAsync(string recipientEmail);
}