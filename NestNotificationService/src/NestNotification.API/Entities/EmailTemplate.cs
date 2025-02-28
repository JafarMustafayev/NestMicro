namespace NestNotification.API.Entities;

public class EmailTemplate : BaseEntity
{
    public string TemplateName { get; set; }
    public string Subject { get; set; }
    public string HtmlBody { get; set; }
    public virtual ICollection<EmailLog> EmailLogs { get; set; }
    public virtual ICollection<FailedEmail> FailedEmails { get; set; }

    public EmailTemplate()
    {
        TemplateName = string.Empty;
        Subject = string.Empty;
        HtmlBody = string.Empty;
    }
}