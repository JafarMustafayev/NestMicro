namespace NestNotification.API.Entities;

public class EmailLog : BaseEntity
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string? TemplateId { get; set; }
    public DateTime SentDate { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public virtual EmailTemplate Template { get; set; }

    public EmailLog()
    {
        To = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        TemplateId = string.Empty;
        ErrorMessage = string.Empty;
        IsSuccess = true;
    }
}