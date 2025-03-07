namespace NestNotification.API.Abstractions.Services;

public interface IEmailService
{
    Task<string> EmailSender(MailRequest request);

    Task<ResponseDto> SendEmailAsync(SendEmailDto emailDto);

    Task<ResponseDto> SendTemplatedEmailAsync(SendTemplatedEmailDto emailDto);

    Task<ResponseDto> SendBulkEmailAsync(SendBulkEmailDto emailDto);

    Task<ResponseDto> SendBulkTemplatedEmailAsync(SendBulkTemplatedEmailDto emailDto);

    ResponseDto GetEmailLogs(int page, int pageSize);

    Task<ResponseDto> GetEmailStatusAsync(string emailId);

    Task<ResponseDto> ResendFailedEmailAsync(string emailId);

    Task<ResponseDto> RetryAllFailedEmailsAsync(int maxRetries = 3);

    Task<ResponseDto> CancelPendingEmailAsync(string emailId);

    Task<ResponseDto> CancelAllPendingEmailsAsync(string recipientEmail);

    Task<ResponseDto> SendPriorityEmailAsync(SendEmailDto emailDto);

    Task<ResponseDto> SendPriorityTemplatedEmailAsync(SendTemplatedEmailDto emailDto);

    Task<ResponseDto> ScheduleEmailAsync(SendScheduledEmailDto sendScheduledEmailDto);

    Task<ResponseDto> ScheduleTemplatedEmailAsync(SendScheduledTemplateEmailDto sendScheduledTemplateEmailDto);

    Task<ResponseDto> ScheduleBulkEmailAsync(SendScheduleBulkEmailDto sendScheduleBulkEmailDto);

    Task<ResponseDto> ScheduleBulkTemplatedEmailAsync(SendScheduleBulkTemplatedEmailDto sendScheduleBulkTemplatedEmailDto);

    public Task ProcessEmailQueueAsync(CancellationToken cancellationToken);
}