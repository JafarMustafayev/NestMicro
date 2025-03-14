namespace NestNotification.API.Entities;

public class EmailLog : BaseEntityId
{
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime SentAt { get; set; }

    public EmailLog()
    {
        ToEmail = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        SentAt = DateTime.UtcNow;
    }
}