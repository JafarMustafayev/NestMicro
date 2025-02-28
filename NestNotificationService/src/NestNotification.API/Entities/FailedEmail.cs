namespace NestNotification.API.Entities;

public class FailedEmail : BaseEntity
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string? TemplateId { get; set; }
    public int RetryCount { get; set; }
    public DateTime LastAttempt { get; set; }

    public virtual EmailTemplate Template { get; set; }

    public FailedEmail()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        TemplateId = string.Empty;
        RetryCount = 0;
        LastAttempt = DateTime.UtcNow;
    }
}