namespace NestNotification.API.Entities;

public class EmailTemplate : BaseEntity
{
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public bool IsHtml { get; set; }

    public EmailTemplate()
    {
        TemplateName = string.Empty;
        Subject = string.Empty;
        Body = string.Empty;
        IsHtml = false;
    }
}