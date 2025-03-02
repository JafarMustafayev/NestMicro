namespace NestNotification.API.Entities;

public class EmailTemplate : BaseEntityID
{
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }
    public DateTime CreatedAt { get; set; }

    public EmailTemplate()
    {
        TemplateName = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
        CreatedAt = DateTime.UtcNow;
    }
}