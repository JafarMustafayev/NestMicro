namespace NestNotification.API.Entities;

public class EmailQueue : BaseEntityID
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public int RetryCount { get; set; }
    public string? ErrorMessage { get; set; }
    public EmailPriority Priority { get; set; }
    public EmailStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastAttempt { get; set; }
    public DateTime? ScheduledAt { get; set; }

    public EmailQueue()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        RetryCount = 0;
        ErrorMessage = string.Empty;
        Status = EmailStatus.Pending;
        Priority = EmailPriority.Normal;
        CreatedAt = DateTime.UtcNow;
        LastAttempt = DateTime.UtcNow;
        ScheduledAt = null;
    }
}